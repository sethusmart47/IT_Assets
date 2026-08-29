using IT_asserts_Claim.entity;

namespace IT_asserts.Repositories.Interface
{
    public interface IAssetRepository
    {
        Task<List<Asset>> GetAllAsync(string? status, string? category, string? brand, string? search);
        Task<Asset?> GetByIdWithDetailsAsync(Guid id);
        Task<Asset?> GetByIdAsync(Guid id);
        Task<bool> SerialNumberExistsAsync(string serialNumber);
        Task<List<string>> GetExistingSerialNumbersAsync(List<string> serialNumbers);
        Task<string?> GetLastAssetTagAsync(string pattern);
        Task<int> GetRegisteredCountByPurchaseAsync(Guid purchaseId);
        Task<int> GetRegisteredCountByPurchaseItemAsync(Guid purchaseId, string category, string brand, string model);
        Task AddAsync(Asset asset);

        Task<Asset?> GetAvailableBySerialNumberAsync(string serialNumber);
        Task<Asset?> GetAvailableByAssetTagAsync(string assetTag);
        //Task<Asset?> GetByIdAsync(Guid id);
        Task AddLifecycleHistoryAsync(AssetLifecycleHistory history);
        Task AddRangeAsync(List<Asset> assets);
        Task SaveChangesAsync();
    }
}
