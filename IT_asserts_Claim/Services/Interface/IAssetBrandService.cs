using ITAssetManagement.Dtos.Asset_Brand;

namespace ITAssetManagement.Services.Interface
{
    public interface IAssetBrandService
    {
        Task<List<AssetBrandDetails>> GetAllAsync();
        Task<AssetBrandDetails?> GetByIdAsync(Guid id);
        Task<List<AssetBrandDetails>> GetByCategoryIdAsync(Guid categoryId);
        Task<AssetBrandDetails> CreateAsync(AssetBrandCreateRequest dto);
        Task<AssetBrandDetails?> UpdateAsync(Guid id, AssetBrandUpdateRequest dto);
        Task<bool> DeleteAsync(Guid id);
    }
}
