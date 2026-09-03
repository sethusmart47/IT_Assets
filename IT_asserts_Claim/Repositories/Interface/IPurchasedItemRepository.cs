using ITAssetManagement.Domain.Entities;

namespace ITAssetManagement.Repositories.Interface
{
    public interface IPurchasedItemRepository
    {
        Task<List<PurchasedItem>> GetAllByPurchaseIdAsync(Guid purchaseId);
        Task<PurchasedItem?> GetPurchasedItemByIdAsync(Guid itemId);
        Task AddPurchasedItemAsync(PurchasedItem entity);
        void UpdatePurchasedItem(PurchasedItem entity);
        Task<int> SaveChangesAsync();

        // Generic wrappers for backward compatibility
        Task<PurchasedItem?> GetByIdAsync(Guid itemId);
        Task AddAsync(PurchasedItem entity);
        void Update(PurchasedItem entity);
    }
}