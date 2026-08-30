using AutoMapper;
using IT_asserts.Dtos.Service;
using IT_asserts.entity;

namespace IT_asserts.Mapping
{
    public class ServiceRequestProfile : Profile
    {
        public ServiceRequestProfile()
        {
            // Create DTO → Entity
            CreateMap<CreateServiceRequestDto, ServiceRequest>()
                .ForMember(d => d.Status, opt => opt.MapFrom(_ => "Open"))
                .ForMember(d => d.Asset, opt => opt.Ignore());

            // Entity → List DTO
            CreateMap<ServiceRequest, ServiceRequestListDto>()
                .ForMember(d => d.AssetTag, opt => opt.MapFrom(s => s.Asset.AssetTag))
                .ForMember(d => d.SerialNumber, opt => opt.MapFrom(s => s.Asset.SerialNumber))
                .ForMember(d => d.Category, opt => opt.MapFrom(s => s.Asset.Category))
                .ForMember(d => d.Brand, opt => opt.MapFrom(s => s.Asset.Brand))
                .ForMember(d => d.Model, opt => opt.MapFrom(s => s.Asset.Model));

            // Entity → Detail DTO
            CreateMap<ServiceRequest, ServiceRequestDto>()
                .ForMember(d => d.AssetTag, opt => opt.MapFrom(s => s.Asset.AssetTag))
                .ForMember(d => d.SerialNumber, opt => opt.MapFrom(s => s.Asset.SerialNumber))
                .ForMember(d => d.Category, opt => opt.MapFrom(s => s.Asset.Category))
                .ForMember(d => d.Brand, opt => opt.MapFrom(s => s.Asset.Brand))
                .ForMember(d => d.Model, opt => opt.MapFrom(s => s.Asset.Model))
                .ForMember(d => d.Configuration, opt => opt.MapFrom(s => s.Asset.Configuration));
        }
    }
}
