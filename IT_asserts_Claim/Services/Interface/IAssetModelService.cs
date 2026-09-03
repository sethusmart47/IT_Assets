using ITAssetManagement.Dtos.Asset_Model;

namespace ITAssetManagement.Services.Interface
{
    public interface IAssetModelService
    {

        Task<List<AssetModelDetails>> GetAllAsync();
        Task<AssetModelDetails?> GetByIdAsync(Guid id);
        Task<AssetModelDetails> CreateAsync(AssetModelCreateRequest dto);
        Task<AssetModelDetails?> UpdateAsync(Guid id, AssetModelUpdateRequest dto);
        Task<bool> DeleteAsync(Guid id);
    }
}
