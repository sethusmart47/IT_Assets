using IT_asserts.entity;
using IT_asserts.Repositories.Interface;
using IT_asserts_Claim.Data;
using Microsoft.EntityFrameworkCore;

namespace IT_asserts.Repositories.Implementations
{
    public class ServiceRequestRepository : IServiceRequestRepository
    {
        private readonly AppDbContext _context;

        public ServiceRequestRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<ServiceRequest>> GetAllAsync()
        {
            return await _context.ServiceRequests
                .AsNoTracking()
                .Include(s => s.Asset)
                .OrderByDescending(s => s.CreatedAt)
                .ToListAsync();
        }

        public async Task<ServiceRequest?> GetByIdWithAssetAsync(Guid id)
        {
            return await _context.ServiceRequests
                .Include(s => s.Asset)
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task AddAsync(ServiceRequest serviceRequest)
        {
            await _context.ServiceRequests.AddAsync(serviceRequest);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
