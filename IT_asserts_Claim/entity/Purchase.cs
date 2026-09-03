using ITAssetManagement.Domain.Entities;
using ITAssetManagement.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ITAssetManagement.Domain.Entities
{
    public class Purchase:AuditInfo
    {
        [Required]
        [MaxLength(50)]
        public string PurchaseNumber { get; set; } = string.Empty;

        [Required]
        public Guid VendorId { get; set; }

        [Required]
        public DateOnly PurchaseDate { get; set; }

        [Required]
        [MaxLength(100)]
        public string InvoiceNumber { get; set; } = string.Empty;

        [Required]
        public DateOnly InvoiceDate { get; set; }

        [Required]
        public DateOnly ExpectedDeliveryDate { get; set; }

        [Required]
        public OwnershipType OwnershipType { get; set; } = OwnershipType.Company;

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalAmount { get; set; }

        [MaxLength(500)]
        public string? Remarks { get; set; }

        [Required]
        public PurchaseStatus Status { get; set; } = PurchaseStatus.Draft;

        // ─── Navigation ───
        [ForeignKey(nameof(VendorId))]
        public Vendor Vendor { get; set; } = null!;

        public ICollection<PurchasedItem> PurchasedItems { get; set; } = new List<PurchasedItem>();
        public ICollection<PurchaseAttachment> Attachments { get; set; } = new List<PurchaseAttachment>();
    }
}
