using AutoMapper;
using ITAssetManagement.Dtos.Asset_Model;
using ITAssetManagement.Domain.Entities;
using ITAssetManagement.Repositories.Interface;
using ITAssetManagement.Services.Interface;

namespace ITAssetManagement.Services.Implementations
{
    public class AssetModelService: IAssetModelService
    {
        private readonly IAssetModelRepository _repository;
        private readonly IAssetCategoryRepository _categoryRepository;
        private readonly IAssetBrandRepository _brandRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<AssetModelService> _logger;

        public AssetModelService(
            IAssetModelRepository repository,
            IAssetCategoryRepository categoryRepository,
            IAssetBrandRepository brandRepository,
            IMapper mapper,
            ILogger<AssetModelService> logger)
        {
            _repository = repository;
            _categoryRepository = categoryRepository;
            _brandRepository = brandRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<List<AssetModelDetails>> GetAllAsync()
        {
            var entities = await _repository.GetAllAssetModelsAsync();
            return _mapper.Map<List<AssetModelDetails>>(entities);
        }

        public async Task<AssetModelDetails?> GetByIdAsync(Guid id)
        {
            var entity = await _repository.GetAssetModelByIdAsync(id);
            return entity != null ? _mapper.Map<AssetModelDetails>(entity) : null;
        }

        public async Task<AssetModelDetails> CreateAsync(AssetModelCreateRequest dto)
        {
            var category = await _categoryRepository.GetAssetCategoryByIdAsync(dto.AssetCategoryId);
            if (category == null)
                throw new InvalidOperationException("Category does not exist.");

            var brand = await _brandRepository.GetAssetBrandByIdAsync(dto.AssetBrandId);
            if (brand == null)
                throw new InvalidOperationException("Brand does not exist.");

            if (brand.AssetCategoryId != dto.AssetCategoryId)
                throw new InvalidOperationException("Brand does not belong to the selected category.");

            var nameExists = await _repository.IsNameExistsAsync(dto.ModelName.Trim(), dto.AssetBrandId);
            if (nameExists)
                throw new InvalidOperationException($"Model '{dto.ModelName}' already exists under this brand.");

            var entity = _mapper.Map<AssetModel>(dto);
            entity.ModelName = dto.ModelName.Trim();
            entity.AssetCategory = category;
            entity.AssetBrand = brand;

            await _repository.AddAssetModelAsync(entity);
            await _repository.SaveChangesAsync();

            _logger.LogInformation("Model created: {Name}", entity.ModelName);

            return _mapper.Map<AssetModelDetails>(entity);
        }

        public async Task<AssetModelDetails?> UpdateAsync(Guid id, AssetModelUpdateRequest dto)
        {
            var entity = await _repository.GetAssetModelByIdAsync(id);
            if (entity == null) return null;

            var category = await _categoryRepository.GetAssetCategoryByIdAsync(dto.AssetCategoryId);
            if (category == null)
                throw new InvalidOperationException("Category does not exist.");

            var brand = await _brandRepository.GetAssetBrandByIdAsync(dto.AssetBrandId);
            if (brand == null)
                throw new InvalidOperationException("Brand does not exist.");

            if (brand.AssetCategoryId != dto.AssetCategoryId)
                throw new InvalidOperationException("Brand does not belong to the selected category.");

            var nameExists = await _repository.IsNameExistsAsync(dto.ModelName.Trim(), dto.AssetBrandId, id);
            if (nameExists)
                throw new InvalidOperationException($"Model '{dto.ModelName}' already exists under this brand.");

            entity.ModelName = dto.ModelName.Trim();
            entity.AssetCategoryId = dto.AssetCategoryId;
            entity.AssetBrandId = dto.AssetBrandId;
            entity.AssetCategory = category;
            entity.AssetBrand = brand;
            entity.IsActive = dto.IsActive;

            _repository.UpdateAssetModel(entity);
            await _repository.SaveChangesAsync();

            _logger.LogInformation("Model updated: {Id}", id);

            return _mapper.Map<AssetModelDetails>(entity);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var entity = await _repository.GetAssetModelByIdAsync(id);
            if (entity == null) return false;

            entity.IsDeleted = true;
            entity.IsActive = false;

            _repository.UpdateAssetModel(entity);
            await _repository.SaveChangesAsync();

            _logger.LogInformation("Model soft-deleted: {Id}", id);

            return true;
        }
    }
}

