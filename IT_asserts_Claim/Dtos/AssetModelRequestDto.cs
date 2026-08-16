using System.ComponentModel.DataAnnotations;

namespace IT_asserts_Claim.Dtos
{
    public class AssetModelRequestDto
    {
        [Required]
        public Guid AssetBrandId { get; set; }
        [Required]
        public Guid AssetCategoryId { get; set; }
        [Required]
        [MaxLength(150)]
        public string ModelName { get; set; } = string.Empty;
    }
}
