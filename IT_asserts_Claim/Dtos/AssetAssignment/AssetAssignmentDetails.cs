namespace ITAssetManagement.Dtos.AssetAssignment
{
    public class AssignAssetDto
    {
        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "Asset ID is required.")]
        public Guid AssetId { get; set; }

        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "Employee ID is required.")]
        public Guid EmployeeId { get; set; }
    }

    public class ReturnAssetDto
    {
        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "Condition at return is required.")]
        [System.ComponentModel.DataAnnotations.Range(1, 5, ErrorMessage = "Condition must be between 1 (New) and 5 (Damaged).")]
        public int ConditionAtReturn { get; set; }

        [System.ComponentModel.DataAnnotations.MaxLength(500)]
        public string? Remarks { get; set; }
    }

    public class SurrenderAssetItemDto
    {
        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "Assignment ID is required.")]
        public Guid AssignmentId { get; set; }

        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "Condition at return is required.")]
        [System.ComponentModel.DataAnnotations.Range(1, 5, ErrorMessage = "Condition must be between 1 (New) and 5 (Damaged).")]
        public int ConditionAtReturn { get; set; }

        [System.ComponentModel.DataAnnotations.MaxLength(500)]
        public string? Remarks { get; set; }
    }

    public class SurrenderAssetsDto
    {
        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "At least one asset must be selected.")]
        [System.ComponentModel.DataAnnotations.MinLength(1, ErrorMessage = "At least one asset must be selected.")]
        public List<SurrenderAssetItemDto> Assets { get; set; } = new();
    }

    // ─── Response DTOs ───────────────────────────────────────────────────────────────────

    public class AssignmentResponseDto
    {
        public Guid Id { get; set; }
        public Guid AssetId { get; set; }
        public string AssetTag { get; set; } = string.Empty;
        public string SerialNumber { get; set; } = string.Empty;
        public Guid EmployeeId { get; set; }
        public string EmployeeName { get; set; } = string.Empty;
        public string EmployeeEmail { get; set; } = string.Empty;
        public DateTime AssignedDate { get; set; }
    }

    public class ReturnResponseDto
    {
        public Guid AssignmentId { get; set; }
        public Guid AssetId { get; set; }
        public string AssetTag { get; set; } = string.Empty;
        public string SerialNumber { get; set; } = string.Empty;
        public string NewStatus { get; set; } = string.Empty;
        public string ConditionName { get; set; } = string.Empty;
        public DateTime ReturnedDate { get; set; }
    }

    public class EmployeeAssignmentDto
    {
        public Guid AssignmentId { get; set; }
        public Guid AssetId { get; set; }
        public string AssetTag { get; set; } = string.Empty;
        public string SerialNumber { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string Brand { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public string? Configuration { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime AssignedDate { get; set; }
        public DateTime? ReturnedDate { get; set; }
        public int? ConditionAtReturn { get; set; }
        public string? ConditionAtReturnName { get; set; }
        public string? Remarks { get; set; }
    }
}
