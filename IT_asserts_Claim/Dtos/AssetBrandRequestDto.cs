using System.ComponentModel.DataAnnotations;

namespace IT_asserts_Claim.Dtos
{
    public class AssetBrandRequestDto
    {
        [Required]
        public Guid AssetCategoryId { get; set; }

        [Required]
        [MaxLength(100)]
        public string BrandName { get; set; } = string.Empty;
    }
}

