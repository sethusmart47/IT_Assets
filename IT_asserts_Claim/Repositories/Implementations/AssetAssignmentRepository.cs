using ITAssetManagement.Domain.Entities;
using ITAssetManagement.Repositories.Interface;
using ITAssetManagement.Data;
using Microsoft.EntityFrameworkCore;

namespace ITAssetManagement.Repositories.Implementations
{
    public class AssetAssignmentRepository : Repository<AssetAssignment>, IAssetAssignmentRepository
    {
        public AssetAssignmentRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<AssetAssignment?> GetAssetAssignmentByIdWithDetailsAsync(Guid id)
        {
            return await Context.AssetAssignments
                .Include(a => a.Asset)
                .Include(a => a.Employee)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<AssetAssignment?> GetActiveAssetAssignmentByAssetIdAsync(Guid assetId)
        {
            return await Context.AssetAssignments
                .Include(a => a.Asset)
                .Include(a => a.Employee)
                .FirstOrDefaultAsync(a => a.AssetId == assetId && a.ReturnedDate == null);
        }

        public async Task<List<AssetAssignment>> GetAllAssetAssignmentsByEmployeeIdAsync(Guid employeeId)
        {
            return await Context.AssetAssignments
                .AsNoTracking()
                .Include(a => a.Asset)
                .Where(a => a.EmployeeId == employeeId)
                .OrderByDescending(a => a.AssignedDate)
                .ToListAsync();
        }

        public async Task<int> GetActiveAssetAssignmentCountByEmployeeIdAsync(Guid employeeId)
        {
            return await Context.AssetAssignments
                .CountAsync(a => a.EmployeeId == employeeId && a.ReturnedDate == null);
        }

        public async Task AddAssetAssignmentAsync(AssetAssignment assignment)
        {
            await Context.AssetAssignments.AddAsync(assignment);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await Context.SaveChangesAsync();
        }
    }
}
