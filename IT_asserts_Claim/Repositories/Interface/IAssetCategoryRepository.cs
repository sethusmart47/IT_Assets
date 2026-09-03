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
    }
}