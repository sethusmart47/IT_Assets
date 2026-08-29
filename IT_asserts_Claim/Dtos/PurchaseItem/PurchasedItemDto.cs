namespace IT_asserts_Claim.Dtos.PurchaseItem
{
    public class PurchasedItemDto
    {
        public Guid Id { get; set; }
        public Guid PurchaseId { get; set; }
        public string Category { get; set; } = string.Empty;
        public string Brand { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public string Configuration { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        //public string WarrantyPeriod { get; set; } = string.Empty;
        public decimal SubTotal { get; set; }
    }
}
