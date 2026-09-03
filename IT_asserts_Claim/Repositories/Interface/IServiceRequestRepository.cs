using ITAssetManagement.Domain.Entities;

namespace ITAssetManagement.Repositories.Interface
{
    public interface IServiceRequestRepository
    {
        Task<List<ServiceRequest>> GetAllServiceRequestsAsync();
        Task<ServiceRequest?> GetServiceRequestByIdWithAssetAsync(Guid id);
        Task AddServiceRequestAsync(ServiceRequest serviceRequest);
        Task<int> SaveChangesAsync();

        // Generic wrappers for backward compatibility
        Task<List<ServiceRequest>> GetAllAsync();
        Task<ServiceRequest?> GetByIdAsync(Guid id);
        Task AddAsync(ServiceRequest serviceRequest);
    }
}