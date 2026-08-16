using Claim_Form.Entities;

namespace IT_asserts_Claim.entity
{
    public class AssetCategory: AuditInfo
    {
        public string CategoryName { get; set; } = string.Empty;

        public bool IsActive { get; set; } = true;

        public ICollection<AssetBrand> Brands { get; set; }
            = new List<AssetBrand>();
        public ICollection<AssetModel> Models { get; set; }
    }
}
