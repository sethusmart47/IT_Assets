using ITAssetManagement.Domain.Entities;
using ITAssetManagement.Domain.Enums;

namespace ITAssetManagement.Repositories.Interface
{
    public interface IPurchaseRepository
    {
        Task<List<Purchase>> GetAllPurchasesAsync(PurchaseStatus? status = null);
        Task<Purchase?> GetPurchaseByIdAsync(Guid id);
        Task<Purchase?> GetPurchaseByIdWithDetailsAsync(Guid id);
        Task<List<Purchase>> GetCompletedWithItemsAsync();
        Task<Purchase?> GetByIdWithItemsAsync(Guid id);
        Task<bool> IsInvoiceNumberExistsAsync(string invoiceNumber, Guid? excludeId = null);
        Task<string> GetNextPurchaseNumberAsync();
        Task AddPurchaseAsync(Purchase entity);
        void UpdatePurchase(Purchase entity);
        Task<int> SaveChangesAsync();

        // Generic wrappers for backward compatibility
        Task<List<Purchase>> GetAllAsync(PurchaseStatus? status = null);
        Task<Purchase?> GetByIdAsync(Guid id);
        Task<Purchase?> GetByIdWithDetailsAsync(Guid id);
        Task AddAsync(Purchase entity);
        void Update(Purchase entity);
    }
}