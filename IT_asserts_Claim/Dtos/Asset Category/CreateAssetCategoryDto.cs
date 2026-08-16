using System.ComponentModel.DataAnnotations;

namespace IT_asserts_Claim.Dtos.Asset_Category
{
    public class CreateAssetCategoryDto
    {
        [Required]
        [MaxLength(100)]
        public string CategoryName { get; set; } = string.Empty;
    }
}
