using IT_asserts_Claim.Dtos.Purchase;

namespace IT_asserts_Claim.Services.Interface
{
    public interface IPurchaseAttachmentService
    {
        Task<List<PurchaseAttachmentDto>> GetAllByPurchaseIdAsync(Guid purchaseId);
        Task<List<PurchaseAttachmentDto>> UploadAsync(Guid purchaseId, List<IFormFile> files);
        Task<(byte[]? FileData, string? ContentType, string? FileName)> DownloadAsync(Guid attachmentId);
        Task<bool> DeleteAsync(Guid attachmentId);
    }
}
