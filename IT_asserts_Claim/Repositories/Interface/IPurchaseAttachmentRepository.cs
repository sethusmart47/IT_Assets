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
        void Update(PurchaseAttachment entity);
        Task<int> SaveChangesAsync();
    }
}
