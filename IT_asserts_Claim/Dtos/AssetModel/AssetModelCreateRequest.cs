using System.ComponentModel.DataAnnotations;

namespace ITAssetManagement.Dtos.Asset_Model
{
    public class AssetModelCreateRequest
    {
        [Required]
        [MaxLength(150)]
        public string ModelName { get; set; } = string.Empty;

        [Required]
        public Guid AssetCategoryId { get; set; }

        [Required]
        public Guid AssetBrandId { get; set; }
    }
}
