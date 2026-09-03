using ITAssetManagement.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace ITAssetManagement.Dtos.Purchase
{
    public class PurchaseUpdateRequest
    {
        [Required(ErrorMessage = "Vendor is required.")]
        public Guid VendorId { get; set; }

        [Required(ErrorMessage = "Purchase date is required.")]
        public DateOnly PurchaseDate { get; set; }

        [Required(ErrorMessage = "Invoice number is required.")]
        [MaxLength(100)]
        public string InvoiceNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "Invoice date is required.")]
        public DateOnly InvoiceDate { get; set; }

        [Required(ErrorMessage = "Expected delivery date is required.")]
        public DateOnly ExpectedDeliveryDate { get; set; }

        [Required(ErrorMessage = "Ownership type is required.")]
        public OwnershipType OwnershipType { get; set; }

        [MaxLength(500)]
        public string? Remarks { get; set; }
    }
}
