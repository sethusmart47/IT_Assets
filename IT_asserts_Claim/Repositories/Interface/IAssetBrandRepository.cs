using IT_asserts_Claim.entity;

namespace IT_asserts_Claim.Repositories.Interface
{
    public interface IAssetBrandRepository
    {
        Task<List<AssetBrand>> GetAllAsync();
        Task<AssetBrand?> GetByIdAsync(Guid id);
        Task<List<AssetBrand>> GetByCategoryIdAsync(Guid categoryId);
        Task<bool> IsNameExistsAsync(string name, Guid categoryId, Guid? excludeId = null);
        Task AddAsync(AssetBrand entity);
        void Update(AssetBrand entity);
        Task<int> SaveChangesAsync();
    }
}
