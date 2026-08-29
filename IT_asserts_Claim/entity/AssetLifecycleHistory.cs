using Claim_Form.Entities;
using IT_asserts_Claim.Enum;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IT_asserts_Claim.entity
{
    public class AssetLifecycleHistory : AuditInfo
    {
        [Required]
        public Guid AssetId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Action { get; set; } = string.Empty;

        public AssetStatus? OldStatus { get; set; }

        [Required]
        public AssetStatus NewStatus { get; set; }

        [MaxLength(500)]
        public string? Remarks { get; set; }

        [Required]
        [MaxLength(200)]
        public string PerformedBy { get; set; } = string.Empty;

        [Required]
        public DateTime PerformedDate { get; set; } = DateTime.UtcNow;


        
        [MaxLength(150)]
        public string? EmployeeEmail { get; set; }

        [MaxLength(150)]
        public string? EmployeeName { get; set; }

        public Guid? ReferenceId { get; set; }

        [MaxLength(50)]
        public string? ReferenceType { get; set; }

        // ─── Navigation ───
        [ForeignKey(nameof(AssetId))]
        public Asset Asset { get; set; } = null!;
    }
}
