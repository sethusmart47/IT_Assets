using Claim_Form.Entities;

namespace IT_asserts_Claim.entity
{
    public class AssetBrand: AuditInfo
    {
        public Guid AssetCategoryId { get; set; }

        public string BrandName { get; set; } = string.Empty;

        public bool IsActive { get; set; } = true;

        public AssetCategory AssetCategory { get; set; } = null!;

        public ICollection<AssetModel> Models { get; set; }
            = new List<AssetModel>();
    }
}
