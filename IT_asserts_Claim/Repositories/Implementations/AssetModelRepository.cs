using IT_asserts_Claim.Data;
using IT_asserts_Claim.entity;
using IT_asserts_Claim.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace IT_asserts_Claim.Repositories.Implementations
{
    public class AssetModelRepository: IAssetModelRepository
    {
        private readonly AppDbContext _context;

        public AssetModelRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<AssetModel>> GetAllAsync()
        {
            return await _context.AssetModels
                .Include(x => x.AssetCategory)
                .Include(x => x.AssetBrand)
                .OrderBy(x => x.ModelName)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<AssetModel?> GetByIdAsync(Guid id)
        {
            return await _context.AssetModels
                .Include(x => x.AssetCategory)
                .Include(x => x.AssetBrand)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<bool> IsNameExistsAsync(string name, Guid brandId, Guid? excludeId = null)
        {
            return await _context.AssetModels
                .AnyAsync(x => x.ModelName.ToLower() == name.ToLower()
                    && x.AssetBrandId == brandId
                    && (!excludeId.HasValue || x.Id != excludeId.Value));
        }

        public async Task AddAsync(AssetModel entity)
        {
            await _context.AssetModels.AddAsync(entity);
        }

        public void Update(AssetModel entity)
        {
            _context.AssetModels.Update(entity);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
