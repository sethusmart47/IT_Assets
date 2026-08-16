using AutoMapper;
using IT_asserts_Claim.Dtos.Asset_Brand;
using IT_asserts_Claim.Dtos.Asset_Category;
using IT_asserts_Claim.Dtos.Asset_Model;
using IT_asserts_Claim.entity;
using static IT_asserts_Claim.Dtos.AssetCascading;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace IT_asserts_Claim.Mapping
{
    public class AssetMappingProfile : Profile
    {

        public AssetMappingProfile()
        {
            // ═══ CATEGORY ═══
            CreateMap<AssetCategory, AssetCategoryDto>();
            CreateMap<CreateAssetCategoryDto, AssetCategory>();
            CreateMap<AssetCategory, CategoryDropdownItem>();

            // ═══ BRAND ═══
            CreateMap<AssetBrand, AssetBrandDto>()
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.AssetCategory.CategoryName));
            CreateMap<CreateAssetBrandDto, AssetBrand>();
            CreateMap<AssetBrand, BrandDropdownItem>();

            // ═══ MODEL ═══
            CreateMap<AssetModel, AssetModelDto>()
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.AssetCategory.CategoryName))
                .ForMember(dest => dest.BrandName, opt => opt.MapFrom(src => src.AssetBrand.BrandName));
            CreateMap<CreateAssetModelDto, AssetModel>();
            CreateMap<AssetModel, ModelDropdownItem>();
        }
    }
}
