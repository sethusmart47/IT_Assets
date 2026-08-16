using IT_asserts_Claim.Dtos.Asset_Category;

namespace IT_asserts_Claim.Services.Interface
{
    public interface IAssetCategoryService
    {
        Task<List<AssetCategoryDto>> GetAllAsync();
        Task<AssetCategoryDto?> GetByIdAsync(Guid id);
        Task<AssetCategoryDto> CreateAsync(CreateAssetCategoryDto dto);
        Task<AssetCategoryDto?> UpdateAsync(Guid id, UpdateAssetCategoryDto dto);
        Task<bool> DeleteAsync(Guid id);
    }
}
