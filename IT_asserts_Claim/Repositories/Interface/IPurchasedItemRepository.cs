using IT_asserts_Claim.entity;

namespace IT_asserts_Claim.Repositories.Interface
{
    public interface IPurchasedItemRepository
    {
        Task<List<PurchasedItem>> GetAllByPurchaseIdAsync(Guid purchaseId);
        Task<PurchasedItem?> GetByIdAsync(Guid itemId);
        Task AddAsync(PurchasedItem entity);
        void Update(PurchasedItem entity);
        Task<int> SaveChangesAsync();
    }
}
