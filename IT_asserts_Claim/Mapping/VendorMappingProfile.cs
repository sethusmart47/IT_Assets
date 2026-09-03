using AutoMapper;
using ITAssetManagement.Dtos.Vendor;
using ITAssetManagement.Domain.Entities;

namespace ITAssetManagement.Mapping
{
    public class VendorMappingProfile : Profile
    {
        public VendorMappingProfile()
        {
            CreateMap<Vendor, VendorDetails>();
            CreateMap<VendorCreateRequest, Vendor>();
            CreateMap<Vendor, VendorDropdownItem>();
        }
    }
}
