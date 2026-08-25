namespace IT_asserts.Dtos.Asset
{
    public class AvailablePurchaseDto
    {

        public Guid Id { get; set; }
        public string PurchaseNumber { get; set; } = string.Empty;
        public string VendorName { get; set; } = string.Empty;
        public DateOnly PurchaseDate { get; set; }
        public int TotalItems { get; set; }
        public int TotalQty { get; set; }
        public int RegisteredQty { get; set; }
        public int RemainingQty { get; set; }
    }

    public class AvailablePurchaseItemDto
    {
        public Guid PurchaseItemId { get; set; }
        public string Category { get; set; } = string.Empty;
        public string Brand { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public string? Configuration { get; set; }
        public decimal UnitPrice { get; set; }
        public string WarrantyMonths { get; set; }
        public int PurchasedQty { get; set; }
        public int RegisteredQty { get; set; }
        public int RemainingQty { get; set; }
    }
}
