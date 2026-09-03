using System.ComponentModel.DataAnnotations;

namespace ITAssetManagement.Dtos.Service
{
    public class ServiceRequestCreateRequest
    {
        [Required(ErrorMessage = "Asset ID is required.")]
        public Guid AssetId { get; set; }

        [Required(ErrorMessage = "Issue type is required.")]
        [MaxLength(100)]
        public string IssueType { get; set; } = string.Empty;

        [Required(ErrorMessage = "Reported date is required.")]
        public DateOnly ReportedDate { get; set; }

        [Required(ErrorMessage = "Priority is required.")]
        [MaxLength(50)]
        public string Priority { get; set; } = string.Empty;

        [MaxLength(200)]
        public string? VendorName { get; set; }

        [MaxLength(2000)]
        public string? IssueDescription { get; set; }
    }
}
