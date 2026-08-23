using IT_asserts_Claim.Dtos.PurchaseItem;

namespace IT_asserts_Claim.Services.Interface
{
    public interface IPurchasedItemService
    {
        Task<List<PurchasedItemDto>> GetAllByPurchaseIdAsync(Guid purchaseId);
        Task<PurchasedItemDto?> GetByIdAsync(Guid itemId);
        Task<PurchasedItemDto> CreateAsync(Guid purchaseId, CreatePurchasedItemDto dto);
        Task<PurchasedItemDto?> UpdateAsync(Guid itemId, UpdatePurchasedItemDto dto);
        Task<bool> DeleteAsync(Guid itemId);
    }
}
