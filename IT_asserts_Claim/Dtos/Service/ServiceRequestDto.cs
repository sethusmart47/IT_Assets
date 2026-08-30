namespace IT_asserts.Dtos.Service
{
    public class ServiceRequestDto
    {
        public Guid Id { get; set; }
        public Guid AssetId { get; set; }
        public string AssetTag { get; set; } = string.Empty;
        public string SerialNumber { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string Brand { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public string? Configuration { get; set; }
        public string IssueType { get; set; } = string.Empty;
        public DateOnly ReportedDate { get; set; }
        public string Priority { get; set; } = string.Empty;
        public string? VendorName { get; set; }
        public string? IssueDescription { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime? ResolvedDate { get; set; }
        public string? ResolutionNotes { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
