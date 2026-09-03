using System.ComponentModel.DataAnnotations;

namespace ITAssetManagement.Dtos.Asset_Category
{
    public class AssetCategoryUpdateRequest
    {
        [Required]
        [MaxLength(100)]
        public string CategoryName { get; set; } = string.Empty;

        public bool IsActive { get; set; }
    }
}
