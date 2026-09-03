using System.ComponentModel.DataAnnotations;

namespace ITAssetManagement.Dtos.Asset_Brand
{
    public class AssetBrandCreateRequest
    {
        [Required]
        [MaxLength(100)]
        public string BrandName { get; set; } = string.Empty;


        [Required]
        public Guid AssetCategoryId { get; set; }



    }
}
