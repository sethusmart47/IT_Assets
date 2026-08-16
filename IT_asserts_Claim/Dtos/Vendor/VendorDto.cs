namespace IT_asserts_Claim.Dtos.Vendor
{
    public class VendorDto
    {
        public Guid Id { get; set; }
        public string VendorName { get; set; } = string.Empty;
        public string ContactPerson { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string? GSTNumber { get; set; }
        public string? Address { get; set; }
        public bool IsActive { get; set; }

    }
}
