using ITAssetManagement.Dtos.Asset_Brand;
using ITAssetManagement.Dtos.Asset_Model;

namespace ITAssetManagement.Dtos.Asset_Category
{
    public class AssetCategoryDetails
    {
        public Guid Id { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public bool IsActive { get; set; }
       
        public ICollection<AssetBrandDetails> AssetBrandDtos { get; set; } = new List<AssetBrandDetails>();
        public ICollection<AssetModelDetails> AssetModelDtos { get; set; } = new List<AssetModelDetails>();
    }
}
