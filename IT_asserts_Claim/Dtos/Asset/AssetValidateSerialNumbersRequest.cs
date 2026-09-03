namespace ITAssetManagement.Dtos.Asset
{
    public class AssetValidateSerialNumbersRequest
    {
        public Guid PurchaseId { get; set; }
        public List<string> SerialNumbers { get; set; } = new();
    }
}
