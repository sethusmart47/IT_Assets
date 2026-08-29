using IT_asserts.Dtos.Asset;
using IT_asserts_Claim.Dtos.Asset;

namespace IT_asserts.Services.Interface
{
    public interface IAssetService
    {
        Task<List<AssetListDto>> GetAllAsync(string? status, string? category, string? brand, string? search);
        Task<AssetDetailDto?> GetByIdAsync(Guid id);
        Task<AssetDto> RegisterAsync(CreateAssetDto dto);
        Task<List<AssetDto>> BulkRegisterAsync(BulkCreateAssetDto dto);
        Task<AssetDto?> UpdateAsync(Guid id, UpdateAssetDto dto);
        Task<bool> DeleteAsync(Guid id);
        Task<ValidationResultDto> ValidateSerialNumbersAsync(ValidateSerialNumbersDto dto);
        Task<List<AvailablePurchaseDto>> GetAvailablePurchasesAsync();
        Task<AssetDetailDto?> SearchAvailableAssetAsync(string? serialNumber, string? assetTag);
        Task<List<AvailablePurchaseItemDto>> GetAvailableItemsAsync(Guid purchaseId);
        Task<RegistrationSummaryDto?> GetRegistrationSummaryAsync(Guid purchaseId);
    }
}
