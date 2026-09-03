using ITAssetManagement.Domain.Enums;

namespace ITAssetManagement.Dtos.Asset
{
    public class AssetListItem
    {
        public Guid Id { get; set; }
        public string AssetTag { get; set; } = string.Empty;
        public string SerialNumber { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string Brand { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public string? Configuration { get; set; }
        public AssetStatus Status { get; set; }
        public string StatusName { get; set; } = string.Empty;
        public AssetCondition Condition { get; set; }
        public string ConditionName { get; set; } = string.Empty;
        public DateOnly WarrantyEndDate { get; set; }
        public bool IsWarrantyActive { get; set; }
        public Guid? CurrentEmployeeId { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
