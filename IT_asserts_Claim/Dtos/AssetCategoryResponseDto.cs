namespace IT_asserts_Claim.Dtos
{
    public class AssetCategoryResponseDto
    {
        public Guid Id { get; set; }
        public string CategoryName { get; set; } = string.Empty;
   
        public bool IsActive { get; set; }
        public ICollection<AssetBrandResponseDto> Brands { get; set; } = new List<AssetBrandResponseDto>();
        public ICollection<AssetModelResponseDto> Models { get; set; } = new List<AssetModelResponseDto>();

    }
}
