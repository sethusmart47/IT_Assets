using ITAssetManagement.Data;
using ITAssetManagement.Domain.Entities;
using ITAssetManagement.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace ITAssetManagement.Repositories.Implementations
{
    public class AssetModelRepository: IAssetModelRepository
    {
        private readonly AppDbContext _context;

        public AssetModelRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<AssetModel>> GetAllAssetModelsAsync()
        {
            return await _context.AssetModels
                .Include(x => x.AssetCategory)
                .Include(x => x.AssetBrand)
                .OrderBy(x => x.ModelName)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<AssetModel?> GetAssetModelByIdAsync(Guid id)
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

        public async Task AddAssetModelAsync(AssetModel entity)
        {
            await _context.AssetModels.AddAsync(entity);
        }

        public void UpdateAssetModel(AssetModel entity)
        {
            _context.AssetModels.Update(entity);
        }

        // Generic wrapper for backward compatibility
        public async Task<List<AssetModel>> GetAllAsync()
            => await GetAllAssetModelsAsync();
        public async Task<AssetModel?> GetByIdAsync(Guid id)
            => await GetAssetModelByIdAsync(id);
        public async Task AddAsync(AssetModel entity)
            => await AddAssetModelAsync(entity);
        public void Update(AssetModel entity)
            => UpdateAssetModel(entity);
        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
