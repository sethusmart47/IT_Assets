using IT_asserts_Claim.entity;

namespace IT_asserts_Claim.Repositories.Interface
{
    public interface IPurchaseAttachmentRepository
    {
        Task<List<PurchaseAttachment>> GetAllByPurchaseIdAsync(Guid purchaseId);
        Task<PurchaseAttachment?> GetByIdAsync(Guid attachmentId);
        Task AddAsync(PurchaseAttachment entity);
        Task AddRangeAsync(List<PurchaseAttachment> entities);
        void Update(PurchaseAttachment entity);
        Task<int> SaveChangesAsync();
    }
}
