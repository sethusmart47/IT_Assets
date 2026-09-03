using ITAssetManagement.Domain.Entities;
using ITAssetManagement.Domain.Enums;

namespace ITAssetManagement.Repositories.Interface
{
    public interface IPurchaseRepository
    {
        Task<List<Purchase>> GetAllPurchasesAsync(PurchaseStatus? status = null);
        Task<Purchase?> GetPurchaseByIdAsync(Guid id);
        Task<Purchase?> GetPurchaseByIdWithDetailsAsync(Guid id);
        Task<List<Purchase>> GetConfirmedWithItemsAsync();
        Task<Purchase?> GetByIdWithItemsAsync(Guid id);
        Task<bool> IsInvoiceNumberExistsAsync(string invoiceNumber, Guid? excludeId = null);
        Task<string> GetNextPurchaseNumberAsync();
        Task<bool> HasPurchasedItemsAsync(Guid purchaseId);
        Task AddPurchaseAsync(Purchase entity);
        void UpdatePurchase(Purchase entity);
        Task<int> SaveChangesAsync();
    }
}
