using IT_asserts_Claim.entity;

namespace IT_asserts_Claim.Repositories.Interface
{
    public interface IAssetModelRepository
    {
        Task<List<AssetModel>> GetAllAsync();
        Task<AssetModel?> GetByIdAsync(Guid id);
        Task<bool> IsNameExistsAsync(string name, Guid brandId, Guid? excludeId = null);
        Task AddAsync(AssetModel entity);
        void Update(AssetModel entity);
        Task<int> SaveChangesAsync();
    }
}
