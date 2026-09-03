using ITAssetManagement.Domain.Entities;
using ITAssetManagement.Repositories.Interface;
using ITAssetManagement.Data;
using Microsoft.EntityFrameworkCore;

namespace ITAssetManagement.Repositories.Implementations
{
    public class AssetAssignmentRepository : IAssetAssignmentRepository
    {
        private readonly AppDbContext _context;

        public AssetAssignmentRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<AssetAssignment?> GetAssetAssignmentByIdWithDetailsAsync(Guid id)
        {
            return await _context.AssetAssignments
                .Include(a => a.Asset)
                .Include(a => a.Employee)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<AssetAssignment?> GetActiveAssetAssignmentByAssetIdAsync(Guid assetId)
        {
            return await _context.AssetAssignments
                .Include(a => a.Asset)
                .Include(a => a.Employee)
                .FirstOrDefaultAsync(a => a.AssetId == assetId && a.ReturnedDate == null);
        }

        public async Task<List<AssetAssignment>> GetAllAssetAssignmentsByEmployeeIdAsync(Guid employeeId)
        {
            return await _context.AssetAssignments
                .AsNoTracking()
                .Include(a => a.Asset)
                .Where(a => a.EmployeeId == employeeId)
                .OrderByDescending(a => a.AssignedDate)
                .ToListAsync();
        }

        public async Task<int> GetActiveAssetAssignmentCountByEmployeeIdAsync(Guid employeeId)
        {
            return await _context.AssetAssignments
                .CountAsync(a => a.EmployeeId == employeeId && a.ReturnedDate == null);
        }

        public async Task AddAssetAssignmentAsync(AssetAssignment assignment)
        {
            await _context.AssetAssignments.AddAsync(assignment);
        }

        // Generic wrappers for backward compatibility
        public async Task<AssetAssignment?> GetByIdWithDetailsAsync(Guid id)
            => await GetAssetAssignmentByIdWithDetailsAsync(id);

        public async Task<AssetAssignment?> GetActiveByAssetIdAsync(Guid assetId)
            => await GetActiveAssetAssignmentByAssetIdAsync(assetId);

        public async Task<List<AssetAssignment>> GetAllByEmployeeIdAsync(Guid employeeId)
            => await GetAllAssetAssignmentsByEmployeeIdAsync(employeeId);

        public async Task<int> GetActiveCountByEmployeeIdAsync(Guid employeeId)
            => await GetActiveAssetAssignmentCountByEmployeeIdAsync(employeeId);

        public async Task AddAsync(AssetAssignment assignment)
            => await AddAssetAssignmentAsync(assignment);

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}