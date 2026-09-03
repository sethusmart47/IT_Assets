using AutoMapper;
using ITAssetManagement.Domain.Entities;
using ITAssetManagement.Domain.Enums;
using ITAssetManagement.Dtos.AssetAssignment;
using ITAssetManagement.Repositories.Interface;
using ITAssetManagement.Services.Interface;

namespace ITAssetManagement.Services.Implementations
{
    public class AssetAssignmentService : IAssetAssignmentService
    {
        private readonly IAssetAssignmentRepository _assignmentRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IAssetRepository _assetRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AssetAssignmentService(
            IAssetAssignmentRepository assignmentRepository,
            IEmployeeRepository employeeRepository,
            IAssetRepository assetRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _assignmentRepository = assignmentRepository;
            _employeeRepository = employeeRepository;
            _assetRepository = assetRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<EmployeeAssignmentDto>> GetAssignmentsByEmployeeIdAsync(Guid employeeId)
        {
            var employee = await _employeeRepository.GetEmployeeByIdAsync(employeeId);
            if (employee == null)
                throw new KeyNotFoundException("Employee not found.");

            var assignments = await _assignmentRepository.GetAllAssetAssignmentsByEmployeeIdAsync(employeeId);
            return _mapper.Map<List<EmployeeAssignmentDto>>(assignments);
        }

        public async Task<AssignmentResponseDto> AssignAssetAsync(AssignAssetDto dto)
        {
            var employee = await _employeeRepository.GetEmployeeByIdAsync(dto.EmployeeId);
            if (employee == null) throw new KeyNotFoundException("Employee not found.");

            var asset = await _assetRepository.GetAssetByIdAsync(dto.AssetId);
            if (asset == null) throw new KeyNotFoundException("Asset not found.");

            if (asset.Status != AssetStatus.Available)
                throw new InvalidOperationException($"Asset '{asset.AssetTag}' is not available. Current status: {asset.Status}");

            var existingAssignment = await _assignmentRepository.GetActiveAssetAssignmentByAssetIdAsync(dto.AssetId);
            if (existingAssignment != null)
                throw new InvalidOperationException($"Asset '{asset.AssetTag}' already has an active assignment.");

            var assignment = new AssetAssignment
            {
                AssetId = dto.AssetId,
                EmployeeId = dto.EmployeeId,
                AssignedDate = DateTime.UtcNow
            };

            await _assignmentRepository.AddAssetAssignmentAsync(assignment);

            asset.Status = AssetStatus.Assigned;
            asset.CurrentEmployeeId = dto.EmployeeId;
            asset.AssignedDate = DateOnly.FromDateTime(DateTime.UtcNow);

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

            await _assetRepository.AddAssetLifecycleHistoryAsync(history);
            await _unitOfWork.SaveChangesAsync();

            var savedAssignment = await _assignmentRepository.GetAssetAssignmentByIdWithDetailsAsync(assignment.Id);
            return _mapper.Map<AssignmentResponseDto>(savedAssignment!);
        }

        public async Task<ReturnResponseDto> ReturnAssetAsync(Guid assignmentId, ReturnAssetDto dto)
        {
            var assignment = await _assignmentRepository.GetAssetAssignmentByIdWithDetailsAsync(assignmentId);
            if (assignment == null) throw new KeyNotFoundException("Assignment not found.");
            if (assignment.ReturnedDate != null) throw new InvalidOperationException("This assignment has already been returned.");

            var asset = assignment.Asset;
            var employee = assignment.Employee;

            var newStatus = GetStatusFromCondition(dto.ConditionAtReturn);
            var newCondition = MapReturnConditionToAssetCondition(dto.ConditionAtReturn);

            assignment.ReturnedDate = DateTime.UtcNow;
            assignment.ConditionAtReturn = dto.ConditionAtReturn;
            assignment.Remarks = dto.Remarks;

            asset.Status = newStatus;
            asset.Condition = newCondition;

            if (newStatus == AssetStatus.Available)
            {
                asset.CurrentEmployeeId = null;
                asset.AssignedDate = null;
            }
            else if (newStatus == AssetStatus.NeedToService || newStatus == AssetStatus.Lost)
            {
                asset.CurrentEmployeeId = null;
                asset.AssignedDate = null;
            }

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

            await _assetRepository.AddAssetLifecycleHistoryAsync(history);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<ReturnResponseDto>(assignment);
        }

        public async Task<List<ReturnResponseDto>> SurrenderAssetsAsync(Guid employeeId, SurrenderAssetsDto dto)
        {
            var employee = await _employeeRepository.GetEmployeeByIdAsync(employeeId);
            if (employee == null) throw new KeyNotFoundException("Employee not found.");

            var results = new List<AssetAssignment>();

            foreach (var item in dto.Assets)
            {
                var assignment = await _assignmentRepository.GetAssetAssignmentByIdWithDetailsAsync(item.AssignmentId);
                if (assignment == null) throw new KeyNotFoundException($"Assignment '{item.AssignmentId}' not found.");
                if (assignment.EmployeeId != employeeId) throw new InvalidOperationException($"Assignment '{item.AssignmentId}' does not belong to this employee.");
                if (assignment.ReturnedDate != null) continue;

                var asset = assignment.Asset;
                var newStatus = GetStatusFromCondition(item.ConditionAtReturn);
                var newCondition = MapReturnConditionToAssetCondition(item.ConditionAtReturn);

                assignment.ReturnedDate = DateTime.UtcNow;
                assignment.ConditionAtReturn = item.ConditionAtReturn;
                assignment.Remarks = item.Remarks;

                asset.Status = newStatus;
                asset.Condition = newCondition;
                asset.CurrentEmployeeId = null;
                asset.AssignedDate = null;

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

                await _assetRepository.AddAssetLifecycleHistoryAsync(history);
                results.Add(assignment);
            }

            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<List<ReturnResponseDto>>(results);
        }

        /// <summary>
        /// Enterprise return-condition mapping: 1=Good → Available, 2=Damaged → NeedToService, 3=Lost → Lost
        /// Aligned with DTO Range(1,3) and UI labels.
        /// </summary>
        private static AssetStatus GetStatusFromCondition(int condition)
        {
            return condition switch
            {
                1 => AssetStatus.Available,
                2 => AssetStatus.NeedToService,
                3 => AssetStatus.Lost,
                _ => throw new InvalidOperationException("Invalid condition value. Must be 1 (Good), 2 (Damaged), or 3 (Lost).")
            };
        }

        private static AssetCondition MapReturnConditionToAssetCondition(int condition)
        {
            return condition switch
            {
                1 => AssetCondition.Good,
                2 => AssetCondition.Damaged,
                3 => AssetCondition.Damaged,
                _ => AssetCondition.Good
            };
        }

        private static string GetConditionName(int condition)
        {
            return condition switch
            {
                1 => "Good",
                2 => "Damaged",
                3 => "Lost",
                _ => "Unknown"
            };
        }
    }
}
