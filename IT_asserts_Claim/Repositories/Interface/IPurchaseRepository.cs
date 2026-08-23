using IT_asserts_Claim.entity;
using IT_asserts_Claim.Enum;

namespace IT_asserts_Claim.Repositories.Interface
{
    public interface IPurchaseRepository
    {
        Task<List<Purchase>> GetAllAsync(PurchaseStatus? status = null);
        Task<Purchase?> GetByIdAsync(Guid id);
        Task<Purchase?> GetByIdWithDetailsAsync(Guid id);
        Task<bool> IsInvoiceNumberExistsAsync(string invoiceNumber, Guid? excludeId = null);
        Task<string> GetNextPurchaseNumberAsync();
        Task AddAsync(Purchase entity);
        void Update(Purchase entity);
        Task<int> SaveChangesAsync();
    }
}
