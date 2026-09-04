using ITAssetManagement.Data;
using ITAssetManagement.Domain.Entities;
using ITAssetManagement.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace ITAssetManagement.Repositories.Implementations
{
    public class AssetBrandRepository : Repository<AssetBrand>, IAssetBrandRepository
    {
        public AssetBrandRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<List<AssetBrand>> GetAllAssetBrandsAsync()
        {
            return await Context.AssetBrands
                .Include(x => x.AssetCategory)
                .OrderBy(x => x.BrandName)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<AssetBrand?> GetAssetBrandByIdAsync(Guid id)
        {
            return await Context.AssetBrands
                .Include(x => x.AssetCategory)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<AssetBrand>> GetByCategoryIdAsync(Guid categoryId)
        {
            return await Context.AssetBrands
                .Include(x => x.AssetCategory)
                .Where(x => x.AssetCategoryId == categoryId && x.IsActive)
                .OrderBy(x => x.BrandName)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<bool> IsNameExistsAsync(string name, Guid categoryId, Guid? excludeId = null)
        {
            return await Context.AssetBrands
                .AnyAsync(x => x.BrandName.ToLower() == name.ToLower()
                    && x.AssetCategoryId == categoryId
                    && (!excludeId.HasValue || x.Id != excludeId.Value));
        }

        public async Task AddAssetBrandAsync(AssetBrand entity) => await AddAsync(entity);

        public void UpdateAssetBrand(AssetBrand entity) => Update(entity);

        public override async Task<int> SaveChangesAsync() => await Context.SaveChangesAsync();
    }
}