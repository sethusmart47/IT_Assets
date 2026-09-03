using ITAssetManagement.Domain.Entities;

namespace ITAssetManagement.Domain.Entities
{
    public class AssetCategory: AuditInfo
    {
        public string CategoryName { get; set; } = string.Empty;

        public bool IsActive { get; set; } = true;

        public ICollection<AssetBrand> Brands { get; set; }
            = new List<AssetBrand>();
        public ICollection<AssetModel> Models { get; set; } = new List<AssetModel>();
    }
}
