using ITAssetManagement.Data;
using ITAssetManagement.Domain.Entities;
using ITAssetManagement.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace ITAssetManagement.Repositories.Implementations
{
    public class AssetBrandRepository: IAssetBrandRepository
    {
        public readonly AppDbContext _context;

        public AssetBrandRepository(AppDbContext dbContext)
        {
            _context = dbContext;
        }

        public async Task<List<AssetBrand>> GetAllAssetBrandsAsync()
        {
            return await _context.AssetBrands
                .Include(x => x.AssetCategory)
                .OrderBy(x => x.BrandName)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<AssetBrand?> GetAssetBrandByIdAsync(Guid id)
        {
            return await _context.AssetBrands
                .Include(x => x.AssetCategory)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<AssetBrand>> GetByCategoryIdAsync(Guid categoryId)
        {
            return await _context.AssetBrands
                .Include(x => x.AssetCategory)
                .Where(x => x.AssetCategoryId == categoryId && x.IsActive)
                .OrderBy(x => x.BrandName)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<bool> IsNameExistsAsync(string name, Guid categoryId, Guid? excludeId = null)
        {
            return await _context.AssetBrands
                .AnyAsync(x => x.BrandName.ToLower() == name.ToLower()
                    && x.AssetCategoryId == categoryId
                    && (!excludeId.HasValue || x.Id != excludeId.Value));
        }

        public async Task AddAssetBrandAsync(AssetBrand entity)
        {
            await _context.AssetBrands.AddAsync(entity);
        }

        public void UpdateAssetBrand(AssetBrand entity)
        {
            _context.AssetBrands.Update(entity);
        }

        // Generic wrapper for backward compatibility
        public async Task<List<AssetBrand>> GetAllAsync()
            => await GetAllAssetBrandsAsync();
        public async Task<AssetBrand?> GetByIdAsync(Guid id)
            => await GetAssetBrandByIdAsync(id);
        public async Task AddAsync(AssetBrand entity)
            => await AddAssetBrandAsync(entity);
        public void Update(AssetBrand entity)
            => UpdateAssetBrand(entity);
        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
