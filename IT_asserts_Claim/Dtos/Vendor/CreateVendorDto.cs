using System.ComponentModel.DataAnnotations;

namespace IT_asserts_Claim.Dtos.Vendor
{
    public class CreateVendorDto
    {
        [Required]
        [MaxLength(200)]
        public string VendorName { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string ContactPerson { get; set; } = string.Empty;

        [Required]
        [MaxLength(150)]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [MaxLength(20)]
        public string Phone { get; set; } = string.Empty;

        [MaxLength(20)]
        public string? GSTNumber { get; set; }

        [MaxLength(500)]
        public string? Address { get; set; }
    }
}
