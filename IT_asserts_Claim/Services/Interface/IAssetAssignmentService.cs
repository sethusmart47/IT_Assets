using IT_asserts.Dtos.AssetAssignment;
using IT_asserts.Dtos.Employee;

namespace IT_asserts.Services.Interface
{
    public interface IAssetAssignmentService
    {
     
        Task<List<EmployeeAssignmentDto>> GetAssignmentsByEmployeeIdAsync(Guid employeeId);
        Task<AssignmentResponseDto> AssignAssetAsync(AssignAssetDto dto);
        Task<ReturnResponseDto> ReturnAssetAsync(Guid assignmentId, ReturnAssetDto dto);
        Task<List<ReturnResponseDto>> SurrenderAssetsAsync(Guid employeeId, SurrenderAssetsDto dto);
    }
}
