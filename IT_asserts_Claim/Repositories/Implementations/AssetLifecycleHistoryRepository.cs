using ITAssetManagement.Repositories.Interface;
using ITAssetManagement.Data;
using ITAssetManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ITAssetManagement.Repositories.Implementations
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

        public async Task AddAssetLifecycleHistoryAsync(AssetLifecycleHistory history)
        {
            await _context.AssetLifecycleHistories.AddAsync(history);
        }

        public async Task AddAssetLifecycleHistoriesAsync(List<AssetLifecycleHistory> histories)
        {
            await _context.AssetLifecycleHistories.AddRangeAsync(histories);
        }

        // Generic wrappers for backward compatibility
        public async Task AddAsync(AssetLifecycleHistory history)
            => await AddAssetLifecycleHistoryAsync(history);

        public async Task AddRangeAsync(List<AssetLifecycleHistory> histories)
            => await AddAssetLifecycleHistoriesAsync(histories);

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}