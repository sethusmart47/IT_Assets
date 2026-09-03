using ITAssetManagement.Domain.Entities;

namespace ITAssetManagement.Repositories.Interface
{
    public interface IAssetCategoryRepository
    {
        Task<List<AssetCategory>> GetAllAssetCategoriesAsync();
        Task<AssetCategory?> GetAssetCategoryByIdAsync(Guid id);
        Task<bool> IsNameExistsAsync(string name, Guid? excludeId = null);
        Task AddAssetCategoryAsync(AssetCategory entity);
        void UpdateAssetCategory(AssetCategory entity);
        Task<int> SaveChangesAsync();

        // Generic wrappers for backward compatibility
        Task<List<AssetCategory>> GetAllAsync();
        Task<AssetCategory?> GetByIdAsync(Guid id);
        Task AddAsync(AssetCategory entity);
        void Update(AssetCategory entity);
    }
}