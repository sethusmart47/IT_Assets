namespace IT_asserts.Dtos.Asset
{
    public class RegistrationSummaryDto
    {
        public Guid PurchaseId { get; set; }
        public string PurchaseNumber { get; set; } = string.Empty;
        public int TotalPurchasedQty { get; set; }
        public int RegisteredQty { get; set; }
        public int RemainingQty { get; set; }
    }
}
