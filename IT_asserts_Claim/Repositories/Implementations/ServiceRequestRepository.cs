using ITAssetManagement.Domain.Entities;
using ITAssetManagement.Repositories.Interface;
using ITAssetManagement.Data;
using Microsoft.EntityFrameworkCore;

namespace ITAssetManagement.Repositories.Implementations
{
    public class ServiceRequestRepository : Repository<ServiceRequest>, IServiceRequestRepository
    {
        public ServiceRequestRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<List<ServiceRequest>> GetAllServiceRequestsAsync()
        {
            return await Context.ServiceRequests
                .AsNoTracking()
                .Include(s => s.Asset)
                .OrderByDescending(s => s.CreatedAt)
                .ToListAsync();
        }

        public async Task<ServiceRequest?> GetServiceRequestByIdWithAssetAsync(Guid id)
        {
            return await Context.ServiceRequests
                .Include(s => s.Asset)
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task AddServiceRequestAsync(ServiceRequest serviceRequest)
        {
            await Context.ServiceRequests.AddAsync(serviceRequest);
        }

        public override async Task<int> SaveChangesAsync()
        {
            return await Context.SaveChangesAsync();
        }
    }
}
