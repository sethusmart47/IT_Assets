namespace IT_asserts_Claim.Dtos.Asset_Category
{
    public class AssetCategoryDto
    {
        public Guid Id { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }
}
