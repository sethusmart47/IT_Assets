using ITAssetManagement.Domain.Enums;

namespace ITAssetManagement.Dtos.Asset
{
    public class AssetDetails
    {
        public Guid Id { get; set; }
        public string AssetTag { get; set; } = string.Empty;
        public string SerialNumber { get; set; } = string.Empty;
        public Guid PurchaseId { get; set; }
        public string Category { get; set; } = string.Empty;
        public string Brand { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public string? Configuration { get; set; }
        public AssetStatus Status { get; set; }
        public string StatusName { get; set; } = string.Empty;
        public AssetCondition Condition { get; set; }
        public string ConditionName { get; set; } = string.Empty;
        public OwnershipType OwnershipType { get; set; }
        public string OwnershipTypeName { get; set; } = string.Empty;
        public DateOnly WarrantyStartDate { get; set; }
        public DateOnly WarrantyEndDate { get; set; }
        public int WarrantyMonths { get; set; }
        public DateOnly? LastServiceDate { get; set; }
        public string? Remarks { get; set; }
        public Guid? CurrentEmployeeId { get; set; }
        public DateOnly? AssignedDate { get; set; }
        public string PurchaseNumber { get; set; } = string.Empty;
        public string VendorName { get; set; } = string.Empty;
        public DateOnly PurchaseDate { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
