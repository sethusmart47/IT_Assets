using AutoMapper;
using ITAssetManagement.Dtos.Asset_Brand;
using ITAssetManagement.Domain.Entities;
using ITAssetManagement.Repositories.Interface;
using ITAssetManagement.Services.Interface;

namespace ITAssetManagement.Services.Implementations
{
    public class AssetBrandService: IAssetBrandService
    {
        private readonly IAssetBrandRepository _repository;
        private readonly IAssetCategoryRepository _categoryRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<AssetBrandService> _logger;

        public AssetBrandService(
            IAssetBrandRepository repository,
            IAssetCategoryRepository categoryRepository,
            IMapper mapper,
            ILogger<AssetBrandService> logger)
        {
            _repository = repository;
            _categoryRepository = categoryRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<List<AssetBrandDetails>> GetAllAsync()
        {
            var entities = await _repository.GetAllAsync();
            return _mapper.Map<List<AssetBrandDetails>>(entities);
        }

        public async Task<AssetBrandDetails?> GetByIdAsync(Guid id)
        {
            var entity = await _repository.GetByIdAsync(id);
            return entity != null ? _mapper.Map<AssetBrandDetails>(entity) : null;
        }

        public async Task<List<AssetBrandDetails>> GetByCategoryIdAsync(Guid categoryId)
        {
            var entities = await _repository.GetByCategoryIdAsync(categoryId);
            return _mapper.Map<List<AssetBrandDetails>>(entities);
        }

        public async Task<AssetBrandDetails> CreateAsync(AssetBrandCreateRequest dto)
        {
            var category = await _categoryRepository.GetAssetCategoryByIdAsync(dto.AssetCategoryId);
            if (category == null)
                throw new InvalidOperationException("Category does not exist.");

            var nameExists = await _repository.IsNameExistsAsync(dto.BrandName.Trim(), dto.AssetCategoryId);
            if (nameExists)
                throw new InvalidOperationException($"Brand '{dto.BrandName}' already exists under this category.");

            //var entity = _mapper.Map<AssetBrand>(dto);
            //entity.BrandName = dto.BrandName.Trim();
            var entity = new AssetBrand
            {
                Id = Guid.NewGuid(),
                BrandName = dto.BrandName.Trim(),
                AssetCategoryId = dto.AssetCategoryId
        
            };
            await _repository.AddAsync(entity);
            await _repository.SaveChangesAsync();

            _logger.LogInformation("Brand created: {Name}", entity.BrandName);

            var reloaded = await _repository.GetByIdAsync(entity.Id);
            return _mapper.Map<AssetBrandDetails>(reloaded!);
        }

        public async Task<AssetBrandDetails?> UpdateAsync(Guid id, AssetBrandUpdateRequest dto)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null) return null;

            var category = await _categoryRepository.GetAssetCategoryByIdAsync(dto.AssetCategoryId);
            if (category == null)
                throw new InvalidOperationException("Category does not exist.");

            var nameExists = await _repository.IsNameExistsAsync(dto.BrandName.Trim(), dto.AssetCategoryId, id);
            if (nameExists)
                throw new InvalidOperationException($"Brand '{dto.BrandName}' already exists under this category.");

            entity.BrandName = dto.BrandName.Trim();
            entity.AssetCategoryId = dto.AssetCategoryId;
            entity.IsActive = dto.IsActive;

            _repository.Update(entity);
            await _repository.SaveChangesAsync();

            _logger.LogInformation("Brand updated: {Id}", id);

            var reloaded = await _repository.GetByIdAsync(id);
            return _mapper.Map<AssetBrandDetails>(reloaded!);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null) return false;

            entity.IsDeleted = true;
            entity.IsActive = false;

            _repository.Update(entity);
            await _repository.SaveChangesAsync();

            _logger.LogInformation("Brand soft-deleted: {Id}", id);

            return true;
        }
    }
}
