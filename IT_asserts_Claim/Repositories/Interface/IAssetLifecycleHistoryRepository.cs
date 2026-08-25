using IT_asserts_Claim.entity;

namespace IT_asserts.Repositories.Interface
{
    public interface IAssetLifecycleHistoryRepository
    {
        Task<List<AssetLifecycleHistory>> GetByAssetIdAsync(Guid assetId);
        Task AddAsync(AssetLifecycleHistory history);
        Task AddRangeAsync(List<AssetLifecycleHistory> histories);
        Task SaveChangesAsync();
    }
}
