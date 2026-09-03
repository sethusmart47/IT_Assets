using ITAssetManagement.Dtos.Service;

namespace ITAssetManagement.Services.Interface
{
    public interface IServiceRequestService
    {
        Task<List<ServiceRequestListItem>> GetAllAsync();
        Task<ServiceRequestDetails?> GetByIdAsync(Guid id);
        Task<ServiceRequestDetails> CreateAsync(ServiceRequestCreateRequest dto);
        Task<ServiceRequestDetails> ResolveAsync(Guid id, ServiceRequestResolveRequest dto);
    }
}
