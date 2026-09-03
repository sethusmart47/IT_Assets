using ITAssetManagement.Domain.Entities;
using ITAssetManagement.Repositories.Interface;
using ITAssetManagement.Data;
using Microsoft.EntityFrameworkCore;

namespace ITAssetManagement.Repositories.Implementations
{
    public class ServiceRequestRepository : IServiceRequestRepository
    {
        private readonly AppDbContext _context;

        public ServiceRequestRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<ServiceRequest>> GetAllServiceRequestsAsync()
        {
            return await _context.ServiceRequests
                .AsNoTracking()
                .Include(s => s.Asset)
                .OrderByDescending(s => s.CreatedAt)
                .ToListAsync();
        }

        public async Task<ServiceRequest?> GetServiceRequestByIdWithAssetAsync(Guid id)
        {
            return await _context.ServiceRequests
                .Include(s => s.Asset)
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task AddServiceRequestAsync(ServiceRequest serviceRequest)
        {
            await _context.ServiceRequests.AddAsync(serviceRequest);
        }

        // Generic wrappers for backward compatibility
        public async Task<List<ServiceRequest>> GetAllAsync()
            => await GetAllServiceRequestsAsync();

        public async Task<ServiceRequest?> GetByIdAsync(Guid id)
            => await GetServiceRequestByIdWithAssetAsync(id);

        public async Task AddAsync(ServiceRequest serviceRequest)
            => await AddServiceRequestAsync(serviceRequest);

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}