using ITAssetManagement.Domain.Entities;

namespace ITAssetManagement.Repositories.Interface
{
    public interface IAssetBrandRepository
    {
        Task<List<AssetBrand>> GetAllAssetBrandsAsync();
        Task<AssetBrand?> GetAssetBrandByIdAsync(Guid id);
        Task<List<AssetBrand>> GetByCategoryIdAsync(Guid categoryId);
        Task<bool> IsNameExistsAsync(string name, Guid categoryId, Guid? excludeId = null);
        Task AddAssetBrandAsync(AssetBrand entity);
        void UpdateAssetBrand(AssetBrand entity);
        Task<int> SaveChangesAsync();

        // Generic wrappers for backward compatibility
        Task<List<AssetBrand>> GetAllAsync();
        Task<AssetBrand?> GetByIdAsync(Guid id);
        Task AddAsync(AssetBrand entity);
        void Update(AssetBrand entity);
    }
}