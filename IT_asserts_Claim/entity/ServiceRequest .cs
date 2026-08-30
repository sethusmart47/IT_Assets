using Claim_Form.Entities;
using IT_asserts_Claim.entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IT_asserts.entity
{
    public class ServiceRequest : AuditInfo
    {
        [Required]
        public Guid AssetId { get; set; }

        [Required]
        [MaxLength(100)]
        public string IssueType { get; set; } = string.Empty;

        [Required]
        public DateOnly ReportedDate { get; set; }

        [Required]
        [MaxLength(50)]
        public string Priority { get; set; } = string.Empty;

        [MaxLength(200)]
        public string? VendorName { get; set; }

        [MaxLength(2000)]
        public string? IssueDescription { get; set; }

        [Required]
        [MaxLength(50)]
        public string Status { get; set; } = "Open";

        public DateTime? ResolvedDate { get; set; }

        [MaxLength(2000)]
        public string? ResolutionNotes { get; set; }

        // Navigation
        [ForeignKey(nameof(AssetId))]
        public Asset Asset { get; set; } = null!;
    }
}
