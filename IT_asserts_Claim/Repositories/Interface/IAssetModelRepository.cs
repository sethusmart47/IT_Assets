using ITAssetManagement.Domain.Entities;

namespace ITAssetManagement.Repositories.Interface
{
    public interface IAssetModelRepository
    {
        Task<List<AssetModel>> GetAllAssetModelsAsync();
        Task<AssetModel?> GetAssetModelByIdAsync(Guid id);
        Task<bool> IsNameExistsAsync(string name, Guid brandId, Guid? excludeId = null);
        Task AddAssetModelAsync(AssetModel entity);
        void UpdateAssetModel(AssetModel entity);
        Task<int> SaveChangesAsync();

        // Generic wrappers for backward compatibility
        Task<List<AssetModel>> GetAllAsync();
        Task<AssetModel?> GetByIdAsync(Guid id);
        Task AddAsync(AssetModel entity);
        void Update(AssetModel entity);
    }
}