using ITAssetManagement.Data;
using ITAssetManagement.Domain.Entities;
using ITAssetManagement.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace ITAssetManagement.Repositories.Implementations
{
    public class AssetModelRepository : Repository<AssetModel>, IAssetModelRepository
    {
        public AssetModelRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<List<AssetModel>> GetAllAssetModelsAsync()
        {
            return await Context.AssetModels
                .Include(x => x.AssetCategory)
                .Include(x => x.AssetBrand)
                .OrderBy(x => x.ModelName)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<AssetModel?> GetAssetModelByIdAsync(Guid id)
        {
            return await Context.AssetModels
                .Include(x => x.AssetCategory)
                .Include(x => x.AssetBrand)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<bool> IsNameExistsAsync(string name, Guid brandId, Guid? excludeId = null)
        {
            return await Context.AssetModels
                .AnyAsync(x => x.ModelName.ToLower() == name.ToLower()
                    && x.AssetBrandId == brandId
                    && (!excludeId.HasValue || x.Id != excludeId.Value));
        }

        public async Task AddAssetModelAsync(AssetModel entity) => await AddAsync(entity);

        public void UpdateAssetModel(AssetModel entity) => Update(entity);

        public async Task<int> SaveChangesAsync() => await Context.SaveChangesAsync();
    }
}