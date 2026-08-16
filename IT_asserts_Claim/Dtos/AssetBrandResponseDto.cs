namespace IT_asserts_Claim.Dtos
{
    public class AssetBrandResponseDto
    {
        public Guid Id { get; set; }

        public Guid AssetCategoryId { get; set; }

        public string CategoryName { get; set; } = string.Empty;

        public string BrandName { get; set; } = string.Empty;

        public bool IsActive { get; set; }
    }
}
