using IT_asserts_Claim.Dtos.Asset_Brand;
using IT_asserts_Claim.Dtos.Asset_Model;

namespace IT_asserts_Claim.Dtos.Asset_Category
{
    public class AssetCategoryDto
    {
        public Guid Id { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public bool IsActive { get; set; }
       
        public ICollection<AssetBrandDto> AssetBrandDtos { get; set; } = new List<AssetBrandDto>();
        public ICollection<AssetModelDto> AssetModelDtos { get; set; } = new List<AssetModelDto>();
    }
}
