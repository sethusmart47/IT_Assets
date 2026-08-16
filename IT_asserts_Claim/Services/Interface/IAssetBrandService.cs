using IT_asserts_Claim.Dtos.Asset_Brand;

namespace IT_asserts_Claim.Services.Interface
{
    public interface IAssetBrandService
    {
        Task<List<AssetBrandDto>> GetAllAsync();
        Task<AssetBrandDto?> GetByIdAsync(Guid id);
        Task<List<AssetBrandDto>> GetByCategoryIdAsync(Guid categoryId);
        Task<AssetBrandDto> CreateAsync(Guid AssetCategoryId,CreateAssetBrandDto dto);
        Task<AssetBrandDto?> UpdateAsync(Guid id, UpdateAssetBrandDto dto);
        Task<bool> DeleteAsync(Guid id);
    }
}
