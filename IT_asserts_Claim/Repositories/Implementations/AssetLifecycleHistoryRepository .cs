using IT_asserts.Repositories.Interface;
using IT_asserts_Claim.Data;
using IT_asserts_Claim.entity;
using Microsoft.EntityFrameworkCore;

namespace IT_asserts.Repositories.Implementations
{
    public class AssetLifecycleHistoryRepository : IAssetLifecycleHistoryRepository
    {
        private readonly AppDbContext _context;

        public AssetLifecycleHistoryRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<AssetLifecycleHistory>> GetByAssetIdAsync(Guid assetId)
        {
            return await _context.AssetLifecycleHistories
                .AsNoTracking()
                .Where(h => h.AssetId == assetId)
                .OrderByDescending(h => h.PerformedDate)
                .ToListAsync();
        }

        public async Task AddAsync(AssetLifecycleHistory history)
        {
            await _context.AssetLifecycleHistories.AddAsync(history);
        }

        public async Task AddRangeAsync(List<AssetLifecycleHistory> histories)
        {
            await _context.AssetLifecycleHistories.AddRangeAsync(histories);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
