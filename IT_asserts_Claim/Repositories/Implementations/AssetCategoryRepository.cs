using ITAssetManagement.Data;
using ITAssetManagement.Domain.Entities;
using ITAssetManagement.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace ITAssetManagement.Repositories.Implementations
{
    public class AssetCategoryRepository: IAssetCategoryRepository
    {
        private readonly AppDbContext _context;
        public AssetCategoryRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<List<AssetCategory>> GetAllAssetCategoriesAsync()
        {
            return await _context.AssetCategories
                .Include(c=>c.Brands)
                  .Include(b=>b.Models)
                .OrderBy(x => x.CategoryName)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<AssetCategory?> GetAssetCategoryByIdAsync(Guid id)
        {
            return await _context.AssetCategories
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<bool> IsNameExistsAsync(string name, Guid? excludeId = null)
        {
            return await _context.AssetCategories
                .AnyAsync(x => x.CategoryName.ToLower() == name.ToLower()
                    && (!excludeId.HasValue || x.Id != excludeId.Value));
        }

        public async Task AddAssetCategoryAsync(AssetCategory entity)
        {
            await _context.AssetCategories.AddAsync(entity);
        }

        public void UpdateAssetCategory(AssetCategory entity)
        {
            _context.AssetCategories.Update(entity);
        }

        // Generic wrappers for backward compatibility
        public async Task<List<AssetCategory>> GetAllAsync()
            => await GetAllAssetCategoriesAsync();

        public async Task<AssetCategory?> GetByIdAsync(Guid id)
            => await GetAssetCategoryByIdAsync(id);

        public async Task AddAsync(AssetCategory entity)
            => await AddAssetCategoryAsync(entity);

        public void Update(AssetCategory entity)
            => UpdateAssetCategory(entity);

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}