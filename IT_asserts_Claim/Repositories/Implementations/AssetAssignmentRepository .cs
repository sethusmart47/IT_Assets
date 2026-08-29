using IT_asserts.entity;
using IT_asserts.Repositories.Interface;
using IT_asserts_Claim.Data;
using Microsoft.EntityFrameworkCore;

namespace IT_asserts.Repositories.Implementations
{
    public class AssetAssignmentRepository : IAssetAssignmentRepository
    {
        private readonly AppDbContext _context;

        public AssetAssignmentRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<AssetAssignment?> GetByIdWithDetailsAsync(Guid id)
        {
            return await _context.AssetAssignments
                .Include(a => a.Asset)
                .Include(a => a.Employee)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<AssetAssignment?> GetActiveByAssetIdAsync(Guid assetId)
        {
            return await _context.AssetAssignments
                .Include(a => a.Asset)
                .Include(a => a.Employee)
                .FirstOrDefaultAsync(a => a.AssetId == assetId && a.ReturnedDate == null);
        }

        public async Task<List<AssetAssignment>> GetAllByEmployeeIdAsync(Guid employeeId)
        {
            return await _context.AssetAssignments
                .AsNoTracking()
                .Include(a => a.Asset)
                .Where(a => a.EmployeeId == employeeId)
                .OrderByDescending(a => a.AssignedDate)
                .ToListAsync();
        }

        public async Task<int> GetActiveCountByEmployeeIdAsync(Guid employeeId)
        {
            return await _context.AssetAssignments
                .CountAsync(a => a.EmployeeId == employeeId && a.ReturnedDate == null);
        }

        public async Task AddAsync(AssetAssignment assignment)
        {
            await _context.AssetAssignments.AddAsync(assignment);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
