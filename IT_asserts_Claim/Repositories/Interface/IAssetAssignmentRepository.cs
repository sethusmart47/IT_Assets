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

        // Generic wrappers for backward compatibility
        Task<AssetAssignment?> GetByIdWithDetailsAsync(Guid id);
        Task<AssetAssignment?> GetActiveByAssetIdAsync(Guid assetId);
        Task<List<AssetAssignment>> GetAllByEmployeeIdAsync(Guid employeeId);
        Task<int> GetActiveCountByEmployeeIdAsync(Guid employeeId);
        Task AddAsync(AssetAssignment assignment);
    }
}