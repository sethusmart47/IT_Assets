using System.ComponentModel.DataAnnotations;

namespace ITAssetManagement.Dtos.Asset_Model
{
    public class AssetModelUpdateRequest
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
