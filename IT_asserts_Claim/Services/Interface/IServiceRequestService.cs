using IT_asserts.Dtos.Service;

namespace IT_asserts.Services.Interface
{
    public interface IServiceRequestService
    {
        Task<List<ServiceRequestListDto>> GetAllAsync();
        Task<ServiceRequestDto?> GetByIdAsync(Guid id);
        Task<ServiceRequestDto> CreateAsync(CreateServiceRequestDto dto);
        Task<ServiceRequestDto> ResolveAsync(Guid id, ResolveServiceRequestDto dto);
    }
}
