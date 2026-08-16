
namespace IT_asserts_Claim.DTOs
{
    // ─── Category DTOs ───────────────────────────────────────────
    public class CreateAssetCategoryDto
    {
        public string CategoryName { get; set; } = string.Empty;
    }

    public class UpdateAssetCategoryDto
    {
        public string CategoryName { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }

    public class AssetCategoryResponseDto
    {
        public Guid Id { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    // ─── Brand DTOs ──────────────────────────────────────────────
    public class CreateAssetBrandDto
    {
        public Guid AssetCategoryId { get; set; }
        public string BrandName { get; set; } = string.Empty;
    }

    public class UpdateAssetBrandDto
    {
        public Guid AssetCategoryId { get; set; }
        public string BrandName { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }

    public class AssetBrandResponseDto
    {
        public Guid Id { get; set; }
        public Guid AssetCategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public string BrandName { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    // ─── Model DTOs ──────────────────────────────────────────────
    public class CreateAssetModelDto
    {
        public Guid AssetCategoryId { get; set; }
        public Guid AssetBrandId { get; set; }
        public string ModelName { get; set; } = string.Empty;
    }

    public class UpdateAssetModelDto
    {
        public Guid AssetCategoryId { get; set; }
        public Guid AssetBrandId { get; set; }
        public string ModelName { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }

    public class AssetModelResponseDto
    {
        public Guid Id { get; set; }
        public Guid AssetCategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public Guid AssetBrandId { get; set; }
        public string BrandName { get; set; } = string.Empty;
        public string ModelName { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    // ─── Dropdown DTO ────────────────────────────────────────────
    public class DropdownDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}

