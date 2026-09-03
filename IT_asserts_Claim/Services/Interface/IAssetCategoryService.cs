using ITAssetManagement.Dtos.Asset_Category;

namespace ITAssetManagement.Services.Interface
{
    public interface IAssetCategoryService
    {
        Task<List<AssetCategoryDetails>> GetAllAsync();
        Task<AssetCategoryDetails?> GetByIdAsync(Guid id);
        Task<AssetCategoryDetails> CreateAsync(AssetCategoryCreateRequest dto);
        Task<AssetCategoryDetails?> UpdateAsync(Guid id, AssetCategoryUpdateRequest dto);
        Task<bool> DeleteAsync(Guid id);
    }
}
