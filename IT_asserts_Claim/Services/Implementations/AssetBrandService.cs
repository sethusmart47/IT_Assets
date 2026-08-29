using AutoMapper;
using IT_asserts_Claim.Dtos.Asset_Brand;
using IT_asserts_Claim.entity;
using IT_asserts_Claim.Repositories.Interface;
using IT_asserts_Claim.Services.Interface;

namespace IT_asserts_Claim.Services.Implementations
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

        public async Task<List<AssetBrandDto>> GetAllAsync()
        {
            var entities = await _repository.GetAllAsync();
            return _mapper.Map<List<AssetBrandDto>>(entities);
        }

        public async Task<AssetBrandDto?> GetByIdAsync(Guid id)
        {
            var entity = await _repository.GetByIdAsync(id);
            return entity != null ? _mapper.Map<AssetBrandDto>(entity) : null;
        }

        public async Task<List<AssetBrandDto>> GetByCategoryIdAsync(Guid categoryId)
        {
            var entities = await _repository.GetByCategoryIdAsync(categoryId);
            return _mapper.Map<List<AssetBrandDto>>(entities);
        }

        public async Task<AssetBrandDto> CreateAsync(CreateAssetBrandDto dto)
        {
            var category = await _categoryRepository.GetByIdAsync(dto.AssetCategoryId);
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
            return _mapper.Map<AssetBrandDto>(reloaded!);
        }

        public async Task<AssetBrandDto?> UpdateAsync(Guid id, UpdateAssetBrandDto dto)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null) return null;

            var category = await _categoryRepository.GetByIdAsync(dto.AssetCategoryId);
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
            return _mapper.Map<AssetBrandDto>(reloaded!);
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
