namespace ITAssetManagement.Dtos.Purchase
{
    public class PurchaseAttachmentDetails
    {
        public Guid Id { get; set; }
        public Guid PurchaseId { get; set; }
        public string FileName { get; set; } = string.Empty;
        public string FilePath { get; set; } = string.Empty;
        public string ContentType { get; set; } = string.Empty;
        public long FileSize { get; set; }
        public string FileSizeDisplay { get; set; } = string.Empty;
       
    }
}
