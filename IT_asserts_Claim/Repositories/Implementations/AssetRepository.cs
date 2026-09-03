using ITAssetManagement.Repositories.Interface;
using ITAssetManagement.Data;
using ITAssetManagement.Domain.Entities;
using ITAssetManagement.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace ITAssetManagement.Repositories.Implementations
{
    public class AssetRepository : Repository<Asset>, IAssetRepository
    {
        public AssetRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<List<Asset>> GetAllAssetsAsync(string? status, string? category, string? brand, string? search)
        {
            var query = Context.Assets.AsNoTracking().AsQueryable();

            if (!string.IsNullOrWhiteSpace(status) && System.Enum.TryParse<AssetStatus>(status, true, out var statusEnum))
                query = query.Where(a => a.Status == statusEnum);

            if (!string.IsNullOrWhiteSpace(category))
                query = query.Where(a => a.Category == category);

            if (!string.IsNullOrWhiteSpace(brand))
                query = query.Where(a => a.Brand == brand);

            if (!string.IsNullOrWhiteSpace(search))
                query = query.Where(a =>
                    a.AssetTag.Contains(search) ||
                    a.SerialNumber.Contains(search) ||
                    a.Model.Contains(search) ||
                    a.Brand.Contains(search) ||
                    a.Category.Contains(search));

            return await query.OrderByDescending(a => a.CreatedAt).ToListAsync();
        }

        public async Task<Asset?> GetAssetByIdWithDetailsAsync(Guid id)
        {
            return await Context.Assets
                
                .Include(a => a.Purchase)
                    .ThenInclude(p => p.Vendor)
                .Include(a => a.LifecycleHistories.OrderByDescending(h => h.PerformedDate))
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<Asset?> GetAssetByIdAsync(Guid id)
        {
            return await Context.Assets
                .Include(a => a.Purchase)
                    .ThenInclude(p => p.Vendor)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<bool> SerialNumberExistsAsync(string serialNumber)
        {
            return await Context.Assets.AnyAsync(a => a.SerialNumber == serialNumber);
        }

        public async Task<List<string>> GetExistingSerialNumbersAsync(List<string> serialNumbers)
        {
            return await Context.Assets
                .Where(a => serialNumbers.Contains(a.SerialNumber))
                .Select(a => a.SerialNumber)
                .ToListAsync();
        }

        public async Task<string?> GetLastAssetTagAsync(string pattern)
        {
            var lastAsset = await Context.Assets
                .IgnoreQueryFilters()
                .Where(a => a.AssetTag.StartsWith(pattern))
                .OrderByDescending(a => a.AssetTag)
                .FirstOrDefaultAsync();

            return lastAsset?.AssetTag;
        }

        public async Task<int> GetRegisteredCountByPurchaseAsync(Guid purchaseId)
        {
            return await Context.Assets.CountAsync(a => a.PurchaseId == purchaseId);
        }

        public async Task<Dictionary<Guid, int>> GetRegisteredCountsByPurchasesAsync(IEnumerable<Guid> purchaseIds)
        {
            var ids = purchaseIds.Distinct().ToList();
            if (ids.Count == 0) return new Dictionary<Guid, int>();

            return await Context.Assets
                .Where(a => ids.Contains(a.PurchaseId))
                .GroupBy(a => a.PurchaseId)
                .Select(g => new { PurchaseId = g.Key, Count = g.Count() })
                .ToDictionaryAsync(x => x.PurchaseId, x => x.Count);
        }

        public async Task<int> GetRegisteredCountByPurchaseItemAsync(Guid purchaseId, string category, string brand, string model)
        {
            return await Context.Assets.CountAsync(a =>
                a.PurchaseId == purchaseId &&
                a.Category == category &&
                a.Brand == brand &&
                a.Model == model);
        }

        public async Task<Dictionary<(string Category, string Brand, string Model), int>> GetRegisteredCountsByPurchaseItemsAsync(Guid purchaseId)
        {
            return await Context.Assets
                .Where(a => a.PurchaseId == purchaseId)
                .GroupBy(a => new { a.Category, a.Brand, a.Model })
                .Select(g => new
                {
                    g.Key.Category,
                    g.Key.Brand,
                    g.Key.Model,
                    Count = g.Count()
                })
                .ToDictionaryAsync(
                    x => (x.Category, x.Brand, x.Model),
                    x => x.Count);
        }
        public async Task<Asset?> GetAvailableBySerialNumberAsync(string serialNumber)
        {
            return await Context.Assets
                .AsNoTracking()
                .Include(a => a.Purchase)
                    .ThenInclude(p => p.Vendor)
                .Include(a => a.LifecycleHistories.OrderByDescending(h => h.PerformedDate))
                .FirstOrDefaultAsync(a => a.SerialNumber.ToLower() == serialNumber.ToLower()
                                       && a.Status == AssetStatus.Available);
        }

        public async Task<Asset?> GetAvailableByAssetTagAsync(string assetTag)
        {
            return await Context.Assets
                .AsNoTracking()
                .Include(a => a.Purchase)
                    .ThenInclude(p => p.Vendor)
                .Include(a => a.LifecycleHistories.OrderByDescending(h => h.PerformedDate))
                .FirstOrDefaultAsync(a => a.AssetTag.ToLower() == assetTag.ToLower()
                                       && a.Status == AssetStatus.Available);
        }
        public async Task<bool> HasActiveAssignmentAsync(Guid assetId)
        {
            return await Context.AssetAssignments
                .AnyAsync(a => a.AssetId == assetId && a.ReturnedDate == null && !a.IsDeleted);
        }


        public async Task<List<Asset>> GetByIdsWithDetailsAsync(List<Guid> ids)
        {
            return await Context.Assets
                .AsNoTracking()
                .Include(a => a.Purchase).ThenInclude(p => p.Vendor)
                .Where(a => ids.Contains(a.Id))
                .OrderBy(a => a.AssetTag)
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

        public async Task AddAssetAsync(Asset asset)
        {
            await Context.Assets.AddAsync(asset);
        }

        public async Task AddAssetsAsync(List<Asset> assets)
        {
            await Context.Assets.AddRangeAsync(assets);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await Context.SaveChangesAsync();
        }
    }
}
