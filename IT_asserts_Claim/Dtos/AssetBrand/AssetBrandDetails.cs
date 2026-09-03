namespace ITAssetManagement.Dtos.Asset_Brand
{
    public class AssetBrandDetails
    {
        public Guid Id { get; set; }
        public string BrandName { get; set; } = string.Empty;
        public Guid AssetCategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }
}
