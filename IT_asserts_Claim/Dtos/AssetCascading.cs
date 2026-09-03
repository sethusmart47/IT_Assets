namespace ITAssetManagement.Dtos
{
    public class AssetCascading
    {
        public class AssetCascadingDropdownDto
        {
            public List<CategoryDropdownItem> Categories { get; set; } = new();
            public List<BrandDropdownItem> Brands { get; set; } = new();
            public List<ModelDropdownItem> Models { get; set; } = new();
        }

        public class CategoryDropdownItem
        {
            public Guid Id { get; set; }
            public string CategoryName { get; set; } = string.Empty;
        }

        public class BrandDropdownItem
        {
            public Guid Id { get; set; }
            public string BrandName { get; set; } = string.Empty;
            public Guid AssetCategoryId { get; set; }
        }

        public class ModelDropdownItem
        {
            public Guid Id { get; set; }
            public string ModelName { get; set; } = string.Empty;
            public Guid AssetCategoryId { get; set; }
            public Guid AssetBrandId { get; set; }
        }
    }
}
