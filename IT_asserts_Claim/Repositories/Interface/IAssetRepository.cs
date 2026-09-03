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
        Task<Dictionary<Guid, int>> GetRegisteredCountsByPurchasesAsync(IEnumerable<Guid> purchaseIds);
        Task<int> GetRegisteredCountByPurchaseItemAsync(Guid purchaseId, string category, string brand, string model);
        Task<Dictionary<(string Category, string Brand, string Model), int>> GetRegisteredCountsByPurchaseItemsAsync(Guid purchaseId);
        Task<Asset?> GetAvailableBySerialNumberAsync(string serialNumber);
        Task<Asset?> GetAvailableByAssetTagAsync(string assetTag);
        Task<List<Asset>> GetByIdsWithDetailsAsync(List<Guid> ids);
        Task<bool> HasActiveAssignmentAsync(Guid assetId);
        Task AddAssetAsync(Asset asset);
        Task AddAssetsAsync(List<Asset> assets);
        Task AddAssetLifecycleHistoryAsync(AssetLifecycleHistory history);
        Task AddAssetLifecycleHistoriesAsync(List<AssetLifecycleHistory> histories);
        Task<int> SaveChangesAsync();
    }
}
