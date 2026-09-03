using ITAssetManagement.Domain.Entities;

namespace ITAssetManagement.Repositories.Interface
{
    public interface IAssetLifecycleHistoryRepository
    {
        Task<List<AssetLifecycleHistory>> GetByAssetIdAsync(Guid assetId);
        Task AddAssetLifecycleHistoryAsync(AssetLifecycleHistory history);
        Task AddAssetLifecycleHistoriesAsync(List<AssetLifecycleHistory> histories);
        Task<int> SaveChangesAsync();

        // Generic wrappers for backward compatibility
        Task AddAsync(AssetLifecycleHistory history);
        Task AddRangeAsync(List<AssetLifecycleHistory> histories);
    }
}