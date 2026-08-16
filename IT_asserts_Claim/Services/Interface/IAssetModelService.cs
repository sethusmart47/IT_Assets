using IT_asserts_Claim.Dtos.Asset_Model;

namespace IT_asserts_Claim.Services.Interface
{
    public interface IAssetModelService
    {

        Task<List<AssetModelDto>> GetAllAsync();
        Task<AssetModelDto?> GetByIdAsync(Guid id);
        Task<AssetModelDto> CreateAsync(CreateAssetModelDto dto);
        Task<AssetModelDto?> UpdateAsync(Guid id, UpdateAssetModelDto dto);
        Task<bool> DeleteAsync(Guid id);
    }
}
