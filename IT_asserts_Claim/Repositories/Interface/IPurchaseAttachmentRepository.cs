using ITAssetManagement.Domain.Entities;

namespace ITAssetManagement.Repositories.Interface
{
    public interface IPurchaseAttachmentRepository
    {
        Task<List<PurchaseAttachment>> GetAllByPurchaseIdAsync(Guid purchaseId);
        Task<PurchaseAttachment?> GetPurchaseAttachmentByIdAsync(Guid attachmentId);
        Task AddPurchaseAttachmentAsync(PurchaseAttachment entity);
        Task AddPurchaseAttachmentsAsync(List<PurchaseAttachment> entities);
        void UpdatePurchaseAttachment(PurchaseAttachment entity);
        Task<int> SaveChangesAsync();

        // Generic wrappers for backward compatibility
        Task<PurchaseAttachment?> GetByIdAsync(Guid attachmentId);
        Task AddAsync(PurchaseAttachment entity);
        Task AddRangeAsync(List<PurchaseAttachment> entities);
        void Update(PurchaseAttachment entity);
    }
}