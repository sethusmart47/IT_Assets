using AutoMapper;
using IT_asserts.Dtos.AssetAssignment;
using IT_asserts.Dtos.Employee;
using IT_asserts.entity;
using IT_asserts.Repositories.Interface;
using IT_asserts.Services.Interface;
using IT_asserts_Claim.entity;
using IT_asserts_Claim.Enum;

namespace IT_asserts.Services.Implementations
{
    public class AssetAssignmentService : IAssetAssignmentService
    {
        private readonly IAssetAssignmentRepository _assignmentRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IAssetRepository _assetRepository;
        private readonly IMapper _mapper;

        public AssetAssignmentService(
            IAssetAssignmentRepository assignmentRepository,
            IEmployeeRepository employeeRepository,
            IAssetRepository assetRepository,
            IMapper mapper)
        {
            _assignmentRepository = assignmentRepository;
            _employeeRepository = employeeRepository;
            _assetRepository = assetRepository;
            _mapper = mapper;
        }

        // ─── Get All Employees (with active asset count) ─────────────────────────────────

        

        // ─── Get All Assignments By Employee (Frontend filters Active/History) ───────────

        public async Task<List<EmployeeAssignmentDto>> GetAssignmentsByEmployeeIdAsync(Guid employeeId)
        {
            var employee = await _employeeRepository.GetByIdAsync(employeeId);
            if (employee == null)
                throw new KeyNotFoundException("Employee not found.");

            var assignments = await _assignmentRepository.GetAllByEmployeeIdAsync(employeeId);
            return _mapper.Map<List<EmployeeAssignmentDto>>(assignments);
        }

        // ─── Assign Asset to Employee ────────────────────────────────────────────────────

        public async Task<AssignmentResponseDto> AssignAssetAsync(AssignAssetDto dto)
        {
            // 1. Validate Employee
            var employee = await _employeeRepository.GetByIdAsync(dto.EmployeeId);
            if (employee == null)
                throw new KeyNotFoundException("Employee not found.");

            // 2. Validate Asset exists
            var asset = await _assetRepository.GetByIdAsync(dto.AssetId);
            if (asset == null)
                throw new KeyNotFoundException("Asset not found.");

            // 3. Validate Asset is Available
            if (asset.Status != AssetStatus.Available)
                throw new InvalidOperationException($"Asset '{asset.AssetTag}' is not available. Current status: {asset.Status}");

            // 4. Validate no active assignment
            var existingAssignment = await _assignmentRepository.GetActiveByAssetIdAsync(dto.AssetId);
            if (existingAssignment != null)
                throw new InvalidOperationException($"Asset '{asset.AssetTag}' already has an active assignment.");

            // 5. Create Assignment
            var assignment = new AssetAssignment
            {
                AssetId = dto.AssetId,
                EmployeeId = dto.EmployeeId,
                AssignedDate = DateTime.UtcNow
            };

            await _assignmentRepository.AddAsync(assignment);

            // 6. Update Asset status
            asset.Status = AssetStatus.Assigned;

            // 7. Create Lifecycle History
            var history = new AssetLifecycleHistory
            {
                AssetId = dto.AssetId,
                Action = "Assigned",
                OldStatus = AssetStatus.Available,
                NewStatus = AssetStatus.Assigned,
                EmployeeName = employee.EmployeeName,
                EmployeeEmail = employee.Email,
                Remarks = $"Assigned to {employee.EmployeeName}",
                PerformedBy = "System",
                PerformedDate = DateTime.UtcNow
            };

            await _assetRepository.AddLifecycleHistoryAsync(history);

            // 8. Save all
            await _assignmentRepository.SaveChangesAsync();

            // 9. Reload with navigation for response
            var savedAssignment = await _assignmentRepository.GetByIdWithDetailsAsync(assignment.Id);
            return _mapper.Map<AssignmentResponseDto>(savedAssignment!);
        }

        // ─── Return Individual Asset ─────────────────────────────────────────────────────

        public async Task<ReturnResponseDto> ReturnAssetAsync(Guid assignmentId, ReturnAssetDto dto)
        {
            // 1. Find assignment
            var assignment = await _assignmentRepository.GetByIdWithDetailsAsync(assignmentId);
            if (assignment == null)
                throw new KeyNotFoundException("Assignment not found.");

            if (assignment.ReturnedDate != null)
                throw new InvalidOperationException("This assignment has already been returned.");

            var asset = assignment.Asset;
            var employee = assignment.Employee;

            // 2. Determine new status
            var newStatus = GetStatusFromCondition(dto.ConditionAtReturn);

            // 3. Close assignment
            assignment.ReturnedDate = DateTime.UtcNow;
            assignment.ConditionAtReturn = dto.ConditionAtReturn;
            assignment.Remarks = dto.Remarks;
            //asset.Condition=dto.ConditionAtReturn.ToString();

            // 4. Update asset status
            asset.Status = newStatus;
            asset.Condition = (AssetCondition)dto.ConditionAtReturn;

            // 5. Lifecycle History
            var history = new AssetLifecycleHistory
            {
                AssetId = asset.Id,
                Action = "Returned",
                OldStatus = AssetStatus.Assigned,
                NewStatus = newStatus,
                EmployeeName = employee.EmployeeName,
                EmployeeEmail = employee.Email,
                Remarks = dto.Remarks ?? $"Returned by {employee.EmployeeName} — Condition: {GetConditionName(dto.ConditionAtReturn)}",
                PerformedBy = "System",
                PerformedDate = DateTime.UtcNow
            };

            await _assetRepository.AddLifecycleHistoryAsync(history);

            // 6. Save
            await _assignmentRepository.SaveChangesAsync();

            return _mapper.Map<ReturnResponseDto>(assignment);
        }

        // ─── Surrender All Assets (Bulk Return) ──────────────────────────────────────────

        public async Task<List<ReturnResponseDto>> SurrenderAssetsAsync(Guid employeeId, SurrenderAssetsDto dto)
        {
            // 1. Validate Employee
            var employee = await _employeeRepository.GetByIdAsync(employeeId);
            if (employee == null)
                throw new KeyNotFoundException("Employee not found.");

            var results = new List<AssetAssignment>();

            // 2. Process each
            foreach (var item in dto.Assets)
            {
                var assignment = await _assignmentRepository.GetByIdWithDetailsAsync(item.AssignmentId);
                if (assignment == null)
                    throw new KeyNotFoundException($"Assignment '{item.AssignmentId}' not found.");

                if (assignment.EmployeeId != employeeId)
                    throw new InvalidOperationException($"Assignment '{item.AssignmentId}' does not belong to this employee.");

                if (assignment.ReturnedDate != null)
                    continue;

                var asset = assignment.Asset;
                var newStatus = GetStatusFromCondition(item.ConditionAtReturn);

                // Close assignment
                assignment.ReturnedDate = DateTime.UtcNow;
                assignment.ConditionAtReturn = item.ConditionAtReturn;
                assignment.Remarks = item.Remarks;

                // Update asset
                asset.Status = newStatus;
                asset.Condition = (AssetCondition)item.ConditionAtReturn;

                // Lifecycle History
                var history = new AssetLifecycleHistory
                {
                    AssetId = asset.Id,
                    Action = "Returned",
                    OldStatus = AssetStatus.Assigned,
                    NewStatus = newStatus,
                    EmployeeName = employee.EmployeeName,
                    EmployeeEmail = employee.Email,
                    Remarks = item.Remarks ?? $"Surrendered by {employee.EmployeeName} — Condition: {GetConditionName(item.ConditionAtReturn)}",
                    PerformedBy = "System",
                    PerformedDate = DateTime.UtcNow
                };

                await _assetRepository.AddLifecycleHistoryAsync(history);
                results.Add(assignment);
            }

            // 3. Save all in one transaction
            await _assignmentRepository.SaveChangesAsync();

            return _mapper.Map<List<ReturnResponseDto>>(results);
        }

        // ─── Private Helpers ─────────────────────────────────────────────────────────────

        private static AssetStatus GetStatusFromCondition(int condition)
        {
            return condition switch
            {
                1 => AssetStatus.Available,
                2 => AssetStatus.Available,
                3 => AssetStatus.Available,
                4 => AssetStatus.Available,// Good
                5 => AssetStatus.NeedToService,     // Damaged
                6 => AssetStatus.Lost,          // Lost
                _ => throw new InvalidOperationException("Invalid condition value. Must be 1 (Good), 2 (Damaged), or 3 (Lost).")
            };
        }

        private static string GetConditionName(int condition)
        {
            return condition switch
            {
                1 => "New",
                2 => "Good",
                3 => "Fair",
                4 => "poor",
                5 => "Damaged",
                6 => "Lost",
                _ => "Unknown"
            };
        }
    }
}
