using ITAssetManagement.Domain.Entities;

namespace ITAssetManagement.Repositories.Interface
{
    public interface IAssetRepository
    {
        Task<List<Asset>> GetAllAssetsAsync(string? status, string? category, string? brand, string? search);
        Task<Asset?> GetAssetByIdWithDetailsAsync(Guid id);
        Task<Asset?> GetAssetByIdAsync(Guid id);
        Task<bool> SerialNumberExistsAsync(string serialNumber);
        Task<List<string>> GetExistingSerialNumbersAsync(List<string> serialNumbers);
        Task<string?> GetLastAssetTagAsync(string pattern);
        Task<int> GetRegisteredCountByPurchaseAsync(Guid purchaseId);
        Task<int> GetRegisteredCountByPurchaseItemAsync(Guid purchaseId, string category, string brand, string model);
        Task<Asset?> GetAvailableBySerialNumberAsync(string serialNumber);
        Task<Asset?> GetAvailableByAssetTagAsync(string assetTag);
        Task<List<Asset>> GetByIdsWithDetailsAsync(List<Guid> ids);
        Task<bool> HasActiveAssignmentAsync(Guid assetId);
        Task AddAssetAsync(Asset asset);
        Task AddAssetsAsync(List<Asset> assets);
        Task AddAssetLifecycleHistoryAsync(AssetLifecycleHistory history);
        Task AddAssetLifecycleHistoriesAsync(List<AssetLifecycleHistory> histories);
        Task<int> SaveChangesAsync();

        // Generic wrappers for backward compatibility
        Task<List<Asset>> GetAllAsync(string? status, string? category, string? brand, string? search);
        Task<Asset?> GetByIdAsync(Guid id);
        Task AddAsync(Asset asset);
        Task AddRangeAsync(List<Asset> assets);
        Task AddLifecycleHistoryAsync(AssetLifecycleHistory history);
    }
}