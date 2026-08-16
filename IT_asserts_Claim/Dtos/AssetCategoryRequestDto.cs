using System.ComponentModel.DataAnnotations;

namespace IT_asserts_Claim.Dtos
{
    public class AssetCategoryRequestDto
    {
        [Required(ErrorMessage = "Category name is required.")]
        [MaxLength(100, ErrorMessage = "Category name cannot exceed 100 characters.")]
        public string Name { get; set; } = string.Empty;

       
    }
}
