using AutoMapper;
using IT_asserts_Claim.Dtos.Vendor;
using IT_asserts_Claim.entity;

namespace IT_asserts_Claim.Mapping
{
    public class VendorMappingProfile : Profile
    {
        public VendorMappingProfile()
        {
            CreateMap<Vendor, VendorDto>();
            CreateMap<CreateVendorDto, Vendor>();
            CreateMap<Vendor, VendorDropdownDto>();
        }
    }
}
