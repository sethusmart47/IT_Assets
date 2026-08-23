using IT_asserts_Claim.Dtos.PurchaseItem;
using IT_asserts_Claim.Enum;

namespace IT_asserts_Claim.Dtos.Purchase
{
    public class PurchaseDetailDto
    {
        public Guid Id { get; set; }
        public string PurchaseNumber { get; set; } = string.Empty;
        public Guid VendorId { get; set; }
        public string VendorName { get; set; } = string.Empty;
        public DateOnly PurchaseDate { get; set; }
        public string InvoiceNumber { get; set; } = string.Empty;
        public DateOnly InvoiceDate { get; set; }
        public DateOnly ExpectedDeliveryDate { get; set; }
        public OwnershipType OwnershipType { get; set; }
        public string OwnershipTypeName { get; set; } = string.Empty;
        public decimal TotalAmount { get; set; }
        public string? Remarks { get; set; }
        public PurchaseStatus Status { get; set; }
        public string StatusName { get; set; } = string.Empty;
        public List<PurchasedItemDto> Items { get; set; } = new();
        public List<PurchaseAttachmentDto> Attachments { get; set; } = new();
    }
}
