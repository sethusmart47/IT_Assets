using ITAssetManagement.Data;
using ITAssetManagement.Domain.Entities;
using ITAssetManagement.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace ITAssetManagement.Repositories.Implementations
{
    public class AssetCategoryRepository : Repository<AssetCategory>, IAssetCategoryRepository
    {
        public AssetCategoryRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<List<AssetCategory>> GetAllAssetCategoriesAsync()
        {
            return await Context.AssetCategories
                .Include(c => c.Brands)
                .Include(b => b.Models)
                .OrderBy(x => x.CategoryName)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<AssetCategory?> GetAssetCategoryByIdAsync(Guid id)
        {
            return await Context.AssetCategories
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<bool> IsNameExistsAsync(string name, Guid? excludeId = null)
        {
            return await Context.AssetCategories
                .AnyAsync(x => x.CategoryName.ToLower() == name.ToLower()
                    && (!excludeId.HasValue || x.Id != excludeId.Value));
        }

        public async Task AddAssetCategoryAsync(AssetCategory entity) => await AddAsync(entity);

        public void UpdateAssetCategory(AssetCategory entity) => Update(entity);

        public async Task<int> SaveChangesAsync() => await Context.SaveChangesAsync();
    }
}