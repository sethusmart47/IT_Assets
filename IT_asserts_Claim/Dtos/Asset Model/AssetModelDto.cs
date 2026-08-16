namespace IT_asserts_Claim.Dtos.Asset_Model
{
    public class AssetModelDto
    {
        public Guid Id { get; set; }
        public string ModelName { get; set; } = string.Empty;
        public Guid AssetCategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public Guid AssetBrandId { get; set; }
        public string BrandName { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }
}
