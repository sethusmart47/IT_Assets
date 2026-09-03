using ITAssetManagement.Repositories.Interface;
using ITAssetManagement.Data;
using ITAssetManagement.Domain.Entities;
using ITAssetManagement.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace ITAssetManagement.Repositories.Implementations
{
    public class AssetRepository : IAssetRepository
    {
        private readonly AppDbContext _context;

        public AssetRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Asset>> GetAllAssetsAsync(string? status, string? category, string? brand, string? search)
        {
            var query = _context.Assets.AsNoTracking().AsQueryable();

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
            return await _context.Assets
                
                .Include(a => a.Purchase)
                    .ThenInclude(p => p.Vendor)
                .Include(a => a.LifecycleHistories.OrderByDescending(h => h.PerformedDate))
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<Asset?> GetAssetByIdAsync(Guid id)
        {
            return await _context.Assets
                .Include(a => a.Purchase)
                    .ThenInclude(p => p.Vendor)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<bool> SerialNumberExistsAsync(string serialNumber)
        {
            return await _context.Assets.AnyAsync(a => a.SerialNumber == serialNumber);
        }

        public async Task<List<string>> GetExistingSerialNumbersAsync(List<string> serialNumbers)
        {
            return await _context.Assets
                .Where(a => serialNumbers.Contains(a.SerialNumber))
                .Select(a => a.SerialNumber)
                .ToListAsync();
        }

        public async Task<string?> GetLastAssetTagAsync(string pattern)
        {
            var lastAsset = await _context.Assets
                .IgnoreQueryFilters()
                .Where(a => a.AssetTag.StartsWith(pattern))
                .OrderByDescending(a => a.AssetTag)
                .FirstOrDefaultAsync();

            return lastAsset?.AssetTag;
        }

        public async Task<int> GetRegisteredCountByPurchaseAsync(Guid purchaseId)
        {
            return await _context.Assets.CountAsync(a => a.PurchaseId == purchaseId);
        }

        public async Task<int> GetRegisteredCountByPurchaseItemAsync(Guid purchaseId, string category, string brand, string model)
        {
            return await _context.Assets.CountAsync(a =>
                a.PurchaseId == purchaseId &&
                a.Category == category &&
                a.Brand == brand &&
                a.Model == model);
        }
        public async Task<Asset?> GetAvailableBySerialNumberAsync(string serialNumber)
        {
            return await _context.Assets
                .AsNoTracking()
                .Include(a => a.Purchase)
                    .ThenInclude(p => p.Vendor)
                .Include(a => a.LifecycleHistories.OrderByDescending(h => h.PerformedDate))
                .FirstOrDefaultAsync(a => a.SerialNumber.ToLower() == serialNumber.ToLower()
                                       && a.Status == AssetStatus.Available);
        }

        public async Task<Asset?> GetAvailableByAssetTagAsync(string assetTag)
        {
            return await _context.Assets
                .AsNoTracking()
                .Include(a => a.Purchase)
                    .ThenInclude(p => p.Vendor)
                .Include(a => a.LifecycleHistories.OrderByDescending(h => h.PerformedDate))
                .FirstOrDefaultAsync(a => a.AssetTag.ToLower() == assetTag.ToLower()
                                       && a.Status == AssetStatus.Available);
        }
        public async Task<bool> HasActiveAssignmentAsync(Guid assetId)
        {
            return await _context.AssetAssignments
                .AnyAsync(a => a.AssetId == assetId && a.ReturnedDate == null && !a.IsDeleted);
        }


        public async Task<List<Asset>> GetByIdsWithDetailsAsync(List<Guid> ids)
        {
            return await _context.Assets
                .AsNoTracking()
                .Include(a => a.Purchase).ThenInclude(p => p.Vendor)
                .Where(a => ids.Contains(a.Id))
                .OrderBy(a => a.AssetTag)
                .ToListAsync();
        }

        public async Task AddLifecycleHistoryAsync(AssetLifecycleHistory history)
        {
            await _context.AssetLifecycleHistories.AddAsync(history);
        }

        public async Task AddLifecycleHistoriesAsync(List<AssetLifecycleHistory> histories)
        {
            await _context.AssetLifecycleHistories.AddRangeAsync(histories);
        }

        // Generic wrapper for backward compatibility
        public async Task AddAssetLifecycleHistoriesAsync(List<AssetLifecycleHistory> histories)
            => await AddLifecycleHistoriesAsync(histories);

        public async Task AddAssetLifecycleHistoryAsync(AssetLifecycleHistory history)
            => await AddLifecycleHistoriesAsync(new List<AssetLifecycleHistory> { history });

        public async Task AddAssetAsync(Asset asset)
        {
            await _context.Assets.AddAsync(asset);
        }

        public async Task AddAssetsAsync(List<Asset> assets)
        {
            await _context.Assets.AddRangeAsync(assets);
        }

        // Generic wrappers for backward compatibility
        public async Task<List<Asset>> GetAllAsync(string? status, string? category, string? brand, string? search)
            => await GetAllAssetsAsync(status, category, brand, search);

        public async Task<Asset?> GetByIdAsync(Guid id)
            => await GetAssetByIdAsync(id);

        public async Task AddAsync(Asset asset)
            => await AddAssetAsync(asset);

        public async Task AddRangeAsync(List<Asset> assets)
            => await AddAssetsAsync(assets);

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}