namespace ITAssetManagement.Dtos.Asset
{
    public class AssetBulkCreateRequest
    {
        public Guid PurchaseId { get; set; }
        public string Category { get; set; } = string.Empty;
        public string Brand { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public string? Configuration { get; set; }
        public int Condition { get; set; } = 1;
        public int OwnershipType { get; set; } = 1;
        public DateOnly WarrantyStartDate { get; set; }
        public DateOnly WarrantyEndDate { get; set; }
        public int WarrantyMonths { get; set; }
        public string? Remarks { get; set; }
        public List<string> SerialNumbers { get; set; } = new();
    }
}
