using ITAssetManagement.Domain.Entities;

namespace ITAssetManagement.Repositories.Interface
{
    public interface IAssetAssignmentRepository
    {
        Task<AssetAssignment?> GetAssetAssignmentByIdWithDetailsAsync(Guid id);
        Task<AssetAssignment?> GetActiveAssetAssignmentByAssetIdAsync(Guid assetId);
        Task<List<AssetAssignment>> GetAllAssetAssignmentsByEmployeeIdAsync(Guid employeeId);
        Task<int> GetActiveAssetAssignmentCountByEmployeeIdAsync(Guid employeeId);
        Task AddAssetAssignmentAsync(AssetAssignment entity);
        Task<int> SaveChangesAsync();
    }
}
