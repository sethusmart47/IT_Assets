using IT_asserts_Claim.entity;

namespace IT_asserts_Claim.Repositories.Interface
{
    public interface IAssetCategoryRepository
    {
        Task<List<AssetCategory>> GetAllAsync();
        Task<AssetCategory?> GetByIdAsync(Guid id);
        Task<bool> IsNameExistsAsync(string name, Guid? excludeId = null);
        Task AddAsync(AssetCategory entity);
        void Update(AssetCategory entity);
        Task<int> SaveChangesAsync();
    }
}