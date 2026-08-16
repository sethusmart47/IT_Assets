using AutoMapper;
using IT_asserts_Claim.Dtos.Asset_Model;
using IT_asserts_Claim.entity;
using IT_asserts_Claim.Repositories.Interface;
using IT_asserts_Claim.Services.Interface;

namespace IT_asserts_Claim.Services.Implementations
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

        public async Task<List<AssetModelDto>> GetAllAsync()
        {
            var entities = await _repository.GetAllAsync();
            return _mapper.Map<List<AssetModelDto>>(entities);
        }

        public async Task<AssetModelDto?> GetByIdAsync(Guid id)
        {
            var entity = await _repository.GetByIdAsync(id);
            return entity != null ? _mapper.Map<AssetModelDto>(entity) : null;
        }

        public async Task<AssetModelDto> CreateAsync(CreateAssetModelDto dto)
        {
            var category = await _categoryRepository.GetByIdAsync(dto.AssetCategoryId);
            if (category == null)
                throw new InvalidOperationException("Category does not exist.");

            var brand = await _brandRepository.GetByIdAsync(dto.AssetBrandId);
            if (brand == null)
                throw new InvalidOperationException("Brand does not exist.");

            if (brand.AssetCategoryId != dto.AssetCategoryId)
                throw new InvalidOperationException("Brand does not belong to the selected category.");

            var nameExists = await _repository.IsNameExistsAsync(dto.ModelName.Trim(), dto.AssetBrandId);
            if (nameExists)
                throw new InvalidOperationException($"Model '{dto.ModelName}' already exists under this brand.");

            var entity = _mapper.Map<AssetModel>(dto);
            entity.ModelName = dto.ModelName.Trim();

            await _repository.AddAsync(entity);
            await _repository.SaveChangesAsync();

            _logger.LogInformation("Model created: {Name}", entity.ModelName);

            var reloaded = await _repository.GetByIdAsync(entity.Id);
            return _mapper.Map<AssetModelDto>(reloaded!);
        }

        public async Task<AssetModelDto?> UpdateAsync(Guid id, UpdateAssetModelDto dto)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null) return null;

            var category = await _categoryRepository.GetByIdAsync(dto.AssetCategoryId);
            if (category == null)
                throw new InvalidOperationException("Category does not exist.");

            var brand = await _brandRepository.GetByIdAsync(dto.AssetBrandId);
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
            entity.IsActive = dto.IsActive;

            _repository.Update(entity);
            await _repository.SaveChangesAsync();

            _logger.LogInformation("Model updated: {Id}", id);

            var reloaded = await _repository.GetByIdAsync(id);
            return _mapper.Map<AssetModelDto>(reloaded!);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null) return false;

            entity.IsDeleted = true;
            entity.IsActive = false;

            _repository.Update(entity);
            await _repository.SaveChangesAsync();

            _logger.LogInformation("Model soft-deleted: {Id}", id);

            return true;
        }
    }
}
