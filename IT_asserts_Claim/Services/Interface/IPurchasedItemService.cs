using ITAssetManagement.Dtos.PurchaseItem;

namespace ITAssetManagement.Services.Interface
{
    public interface IPurchasedItemService
    {
        Task<List<PurchasedItemDetails>> GetAllByPurchaseIdAsync(Guid purchaseId);
        Task<PurchasedItemDetails?> GetByIdAsync(Guid itemId);
        Task<PurchasedItemDetails> CreateAsync(Guid purchaseId, PurchasedItemCreateRequest dto);
        Task<PurchasedItemDetails?> UpdateAsync(Guid itemId, PurchasedItemUpdateRequest dto);
        Task<bool> DeleteAsync(Guid itemId);
    }
}
