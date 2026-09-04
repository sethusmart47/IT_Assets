using ITAssetManagement.Repositories.Interface;
using ITAssetManagement.Data;
using ITAssetManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ITAssetManagement.Repositories.Implementations
{
    public class AssetLifecycleHistoryRepository : Repository<AssetLifecycleHistory>, IAssetLifecycleHistoryRepository
    {
        public AssetLifecycleHistoryRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<List<AssetLifecycleHistory>> GetByAssetIdAsync(Guid assetId)
        {
            return await Context.AssetLifecycleHistories
                .AsNoTracking()
                .Where(h => h.AssetId == assetId)
                .OrderByDescending(h => h.PerformedDate)
                .ToListAsync();
        }

        public async Task AddAssetLifecycleHistoryAsync(AssetLifecycleHistory history)
        {
            await Context.AssetLifecycleHistories.AddAsync(history);
        }

        public async Task AddAssetLifecycleHistoriesAsync(List<AssetLifecycleHistory> histories)
        {
            await Context.AssetLifecycleHistories.AddRangeAsync(histories);
        }

        public override async Task<int> SaveChangesAsync()
        {
            return await Context.SaveChangesAsync();
        }
    }
}
