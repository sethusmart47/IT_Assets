using AutoMapper;
using ITAssetManagement.Domain.Entities;
using ITAssetManagement.Dtos.Asset;
using ITAssetManagement.Dtos.Asset_Brand;
using ITAssetManagement.Dtos.Asset_Category;
using ITAssetManagement.Dtos.Asset_Model;
using static ITAssetManagement.Dtos.AssetCascading;

namespace ITAssetManagement.Mapping
{
    public class AssetMappingProfile : Profile
    {
        public AssetMappingProfile()
        {
            CreateMap<Asset, AssetListItem>()
                .ForMember(d => d.StatusName, opt => opt.MapFrom(s => s.Status.ToString()))
                .ForMember(d => d.ConditionName, opt => opt.MapFrom(s => s.Condition.ToString()))
                .ForMember(d => d.IsWarrantyActive, opt => opt.MapFrom(s =>
                    s.WarrantyEndDate >= DateOnly.FromDateTime(DateTime.UtcNow)));

            CreateMap<Asset, AssetDetails>()
                .ForMember(d => d.StatusName, opt => opt.MapFrom(s => s.Status.ToString()))
                .ForMember(d => d.ConditionName, opt => opt.MapFrom(s => s.Condition.ToString()))
                .ForMember(d => d.OwnershipTypeName, opt => opt.MapFrom(s => s.OwnershipType.ToString()))
                .ForMember(d => d.PurchaseNumber, opt => opt.MapFrom(s => s.Purchase.PurchaseNumber))
                .ForMember(d => d.VendorName, opt => opt.MapFrom(s => s.Purchase.Vendor.VendorName))
                .ForMember(d => d.PurchaseDate, opt => opt.MapFrom(s => s.Purchase.PurchaseDate));

            CreateMap<Asset, AssetDetailsFull>()
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

            CreateMap<AssetLifecycleHistory, AssetLifecycleHistoryDetails>()
                .ForMember(d => d.OldStatusName, opt => opt.MapFrom(s =>
                    s.OldStatus.HasValue ? s.OldStatus.Value.ToString() : null))
                .ForMember(d => d.NewStatusName, opt => opt.MapFrom(s => s.NewStatus.ToString()));

            // ═══ CATEGORY ═══
            CreateMap<AssetCategory, AssetCategoryDetails>()
                .ForMember(dest => dest.AssetBrandDtos, opt => opt.MapFrom(src => src.Brands))
                .ForMember(dest => dest.AssetModelDtos, opt => opt.MapFrom(src => src.Models));
            CreateMap<AssetCategoryCreateRequest, AssetCategory>();
            CreateMap<AssetCategory, CategoryDropdownItem>();

            // ═══ BRAND ═══
            CreateMap<AssetBrand, AssetBrandDetails>()
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.AssetCategory.CategoryName));
            CreateMap<AssetBrandCreateRequest, AssetBrand>();
            CreateMap<AssetBrand, BrandDropdownItem>();

            // ═══ MODEL ═══
            CreateMap<AssetModel, AssetModelDetails>()
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.AssetCategory.CategoryName))
                .ForMember(dest => dest.BrandName, opt => opt.MapFrom(src => src.AssetBrand.BrandName));
            CreateMap<AssetModelCreateRequest, AssetModel>();
            CreateMap<AssetModel, ModelDropdownItem>();
        }
    }
}
