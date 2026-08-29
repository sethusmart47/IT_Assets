using IT_asserts_Claim.Enum;
using System.ComponentModel.DataAnnotations;

namespace IT_asserts.Dtos.Asset
{
    public class AssetLifecycleHistoryDto
    {
        public Guid Id { get; set; }
        public Guid AssetId { get; set; }
        public string Action { get; set; } = string.Empty;
        public AssetStatus? OldStatus { get; set; }
        public string? OldStatusName { get; set; }
        public AssetStatus NewStatus { get; set; }
        public string NewStatusName { get; set; } = string.Empty;
        public string? Remarks { get; set; }
        public string PerformedBy { get; set; } = string.Empty;
        public DateTime PerformedDate { get; set; }
       
        public string? EmployeeName { get; set; }

        
        public string? EmployeeEmail { get; set; }
        public Guid? ReferenceId { get; set; }
        public string? ReferenceType { get; set; }
    }
}
