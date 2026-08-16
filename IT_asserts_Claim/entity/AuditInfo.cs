namespace Claim_Form.Entities
{
    public abstract class AuditInfo
    {
        public Guid Id { get; set; }

        public string CreatedBy { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }

        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }

        public string? DeletedBy { get; set; }
        public DateTime? DeletedAt { get; set; }

        public bool IsDeleted { get; set; } = false;
    }
}
