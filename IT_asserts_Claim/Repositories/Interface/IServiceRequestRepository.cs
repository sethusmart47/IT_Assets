using IT_asserts.entity;

namespace IT_asserts.Repositories.Interface
{
    public interface IServiceRequestRepository
    {
        Task<List<ServiceRequest>> GetAllAsync();
        Task<ServiceRequest?> GetByIdWithAssetAsync(Guid id);
        Task AddAsync(ServiceRequest serviceRequest);
        Task SaveChangesAsync();
    }
}
