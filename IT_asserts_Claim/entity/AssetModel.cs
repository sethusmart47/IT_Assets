using ITAssetManagement.Domain.Entities;

namespace ITAssetManagement.Domain.Entities
{
    public class AssetModel : AuditInfo
    {
       

        public string ModelName { get; set; } = string.Empty;

        public bool IsActive { get; set; } = true;
        public Guid AssetCategoryId { get; set; }
        public Guid AssetBrandId { get; set; }
        public AssetBrand AssetBrand { get; set; } = null!;

        public AssetCategory AssetCategory { get; set; } = null!;
    }
}
