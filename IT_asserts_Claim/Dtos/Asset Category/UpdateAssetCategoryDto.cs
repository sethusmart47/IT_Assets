using System.ComponentModel.DataAnnotations;

namespace IT_asserts_Claim.Dtos.Asset_Category
{
    public class UpdateAssetCategoryDto
    {
        [Required]
        [MaxLength(100)]
        public string CategoryName { get; set; } = string.Empty;

        public bool IsActive { get; set; }
    }
}
