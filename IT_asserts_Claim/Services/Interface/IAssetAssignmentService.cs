using ITAssetManagement.Dtos.AssetAssignment;
using ITAssetManagement.Dtos.Employee;

namespace ITAssetManagement.Services.Interface
{
    public interface IAssetAssignmentService
    {
     
        Task<List<EmployeeAssignmentDto>> GetAssignmentsByEmployeeIdAsync(Guid employeeId);
        Task<AssignmentResponseDto> AssignAssetAsync(AssignAssetDto dto);
        Task<ReturnResponseDto> ReturnAssetAsync(Guid assignmentId, ReturnAssetDto dto);
        Task<List<ReturnResponseDto>> SurrenderAssetsAsync(Guid employeeId, SurrenderAssetsDto dto);
    }
}
