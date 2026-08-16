using System.ComponentModel.DataAnnotations;

namespace IT_asserts_Claim.Dtos.Asset_Model
{
    public class UpdateAssetModelDto
    {
        [Required]
        [MaxLength(150)]
        public string ModelName { get; set; } = string.Empty;

        [Required]
        public Guid AssetCategoryId { get; set; }

        [Required]
        public Guid AssetBrandId { get; set; }

        public bool IsActive { get; set; }
    }
}
