using ITAssetManagement.Dtos.Purchase;

namespace ITAssetManagement.Services.Interface
{
    public interface IPurchaseAttachmentService
    {
        Task<List<PurchaseAttachmentDetails>> GetAllByPurchaseIdAsync(Guid purchaseId);
        Task<List<PurchaseAttachmentDetails>> UploadAsync(Guid purchaseId, List<IFormFile> files);
        Task<(byte[]? FileData, string? ContentType, string? FileName)> DownloadAsync(Guid attachmentId);
        Task<bool> DeleteAsync(Guid attachmentId);
    }
}
