using IT_asserts_Claim.Data;
using IT_asserts_Claim.entity;
using IT_asserts_Claim.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace IT_asserts_Claim.Repositories.Implementations
{
    public class AssetBrandRepository: IAssetBrandRepository
    {
        public readonly AppDbContext _context;

        public AssetBrandRepository(AppDbContext dbContext)
        {
            _context = dbContext;
        }

        public async Task<List<AssetBrand>> GetAllAsync()
        {
            return await _context.AssetBrands
                .Include(x => x.AssetCategory)
                .OrderBy(x => x.BrandName)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<AssetBrand?> GetByIdAsync(Guid id)
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

        public async Task AddAsync(AssetBrand entity)
        {
            await _context.AssetBrands.AddAsync(entity);
        }

        public void Update(AssetBrand entity)
        {
            _context.AssetBrands.Update(entity);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
