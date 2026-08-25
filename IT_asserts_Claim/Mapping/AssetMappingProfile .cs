using AutoMapper;
using IT_asserts.Dtos.Asset;
using IT_asserts_Claim.Dtos.Asset;
using IT_asserts_Claim.entity;

namespace IT_asserts.Mapping
{
    public class AssetMappingProfile : Profile
    {
        public AssetMappingProfile()
        {
            CreateMap<Asset, AssetListDto>()
                .ForMember(d => d.StatusName, opt => opt.MapFrom(s => s.Status.ToString()))
                .ForMember(d => d.ConditionName, opt => opt.MapFrom(s => s.Condition.ToString()))
                .ForMember(d => d.IsWarrantyActive, opt => opt.MapFrom(s =>
                    s.WarrantyEndDate >= DateOnly.FromDateTime(DateTime.UtcNow)));

            CreateMap<Asset, AssetDto>()
                .ForMember(d => d.StatusName, opt => opt.MapFrom(s => s.Status.ToString()))
                .ForMember(d => d.ConditionName, opt => opt.MapFrom(s => s.Condition.ToString()))
                .ForMember(d => d.OwnershipTypeName, opt => opt.MapFrom(s => s.OwnershipType.ToString()))
                .ForMember(d => d.PurchaseNumber, opt => opt.MapFrom(s => s.Purchase.PurchaseNumber))
                .ForMember(d => d.VendorName, opt => opt.MapFrom(s => s.Purchase.Vendor.VendorName))
                .ForMember(d => d.PurchaseDate, opt => opt.MapFrom(s => s.Purchase.PurchaseDate));

            CreateMap<Asset, AssetDetailDto>()
                .ForMember(d => d.StatusName, opt => opt.MapFrom(s => s.Status.ToString()))
                .ForMember(d => d.ConditionName, opt => opt.MapFrom(s => s.Condition.ToString()))
                .ForMember(d => d.OwnershipTypeName, opt => opt.MapFrom(s => s.OwnershipType.ToString()))
                .ForMember(d => d.IsWarrantyActive, opt => opt.MapFrom(s =>
                    s.WarrantyEndDate >= DateOnly.FromDateTime(DateTime.UtcNow)))
                .ForMember(d => d.WarrantyDaysRemaining, opt => opt.MapFrom(s =>
                    Math.Max(0, s.WarrantyEndDate.DayNumber - DateOnly.FromDateTime(DateTime.UtcNow).DayNumber)))
                .ForMember(d => d.PurchaseNumber, opt => opt.MapFrom(s => s.Purchase.PurchaseNumber))
                .ForMember(d => d.VendorName, opt => opt.MapFrom(s => s.Purchase.Vendor.VendorName))
                .ForMember(d => d.PurchaseDate, opt => opt.MapFrom(s => s.Purchase.PurchaseDate))
                .ForMember(d => d.LifecycleHistories, opt => opt.MapFrom(s => s.LifecycleHistories));

            CreateMap<AssetLifecycleHistory, AssetLifecycleHistoryDto>()
                .ForMember(d => d.OldStatusName, opt => opt.MapFrom(s =>
                    s.OldStatus.HasValue ? s.OldStatus.Value.ToString() : null))
                .ForMember(d => d.NewStatusName, opt => opt.MapFrom(s => s.NewStatus.ToString()));
        }
    }
}
