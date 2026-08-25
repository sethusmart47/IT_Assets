using Claim_Form.Entities;
using IT_asserts_Claim.Enum;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IT_asserts_Claim.entity
{
    public class Asset : AuditInfo
    {
        [Required]
        [MaxLength(50)]
        public string AssetTag { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string SerialNumber { get; set; } = string.Empty;

        [Required]
        public Guid PurchaseId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Category { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string Brand { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string Model { get; set; } = string.Empty;

        [MaxLength(250)]
        public string? Configuration { get; set; }

        [Required]
        public AssetStatus Status { get; set; } = AssetStatus.Available;

        [Required]
        public AssetCondition Condition { get; set; } = AssetCondition.New;

        [Required]
        public OwnershipType OwnershipType { get; set; } = OwnershipType.Company;

        [Required]
        public DateOnly WarrantyStartDate { get; set; }

        [Required]
        public DateOnly WarrantyEndDate { get; set; }

        public int WarrantyMonths { get; set; }

        public DateOnly? LastServiceDate { get; set; }

        [MaxLength(500)]
        public string? Remarks { get; set; }

        public Guid? CurrentEmployeeId { get; set; }

        public DateOnly? AssignedDate { get; set; }

        // ─── Navigation ───
        [ForeignKey(nameof(PurchaseId))]
        public Purchase Purchase { get; set; } = null!;

        public ICollection<AssetLifecycleHistory> LifecycleHistories { get; set; } = new List<AssetLifecycleHistory>();
    }
}
