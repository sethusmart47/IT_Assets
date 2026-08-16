using IT_asserts_Claim.Data;
using IT_asserts_Claim.entity;
using IT_asserts_Claim.Repositories.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Logging;

namespace IT_asserts_Claim.Repositories.Implementations
{
    public class AssetCategoryRepository: IAssetCategoryRepository
    {
        private readonly AppDbContext _context;
        public AssetCategoryRepository(AppDbContext context)
        {
            _context = context;

        }
        public async Task<List<AssetCategory>> GetAllAsync()
        {
            return await _context.AssetCategories
                .OrderBy(x => x.CategoryName)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<AssetCategory?> GetByIdAsync(Guid id)
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

        public async Task AddAsync(AssetCategory entity)
        {
            await _context.AssetCategories.AddAsync(entity);
        }

        public void Update(AssetCategory entity)
        {
            _context.AssetCategories.Update(entity);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
