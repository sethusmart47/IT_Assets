using IT_asserts.entity;

namespace IT_asserts.Repositories.Interface
{
    public interface IAssetAssignmentRepository
    {
        Task<AssetAssignment?> GetByIdWithDetailsAsync(Guid id);
        Task<AssetAssignment?> GetActiveByAssetIdAsync(Guid assetId);
        Task<List<AssetAssignment>> GetAllByEmployeeIdAsync(Guid employeeId);
        Task<int> GetActiveCountByEmployeeIdAsync(Guid employeeId);
        Task AddAsync(AssetAssignment assignment);
        Task SaveChangesAsync();
    }
}
