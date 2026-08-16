using System.ComponentModel.DataAnnotations;

namespace IT_asserts_Claim.Dtos.Asset_Brand
{
    public class UpdateAssetBrandDto
    {
        [Required]
        [MaxLength(100)]
        public string BrandName { get; set; } = string.Empty;

        [Required]
        public Guid AssetCategoryId { get; set; }

        public bool IsActive { get; set; }
    }
}
