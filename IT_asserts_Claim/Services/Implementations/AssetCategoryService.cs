using AutoMapper;
using ITAssetManagement.Dtos.Asset_Category;
using ITAssetManagement.Domain.Entities;
using ITAssetManagement.Repositories.Interface;
using ITAssetManagement.Services.Interface;
using Microsoft.Extensions.Logging;

namespace ITAssetManagement.Services.Implementations
{
    public class AssetCategoryService: IAssetCategoryService
    {
        private readonly IAssetCategoryRepository _assetCategoryRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<AssetCategoryService> _logger;

        public AssetCategoryService(IAssetCategoryRepository assetCategoryRepository, IMapper mapper, ILogger<AssetCategoryService> logger)
        {
            _assetCategoryRepository = assetCategoryRepository;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<List<AssetCategoryDetails>> GetAllAsync()
        {
            var entities = await _assetCategoryRepository.GetAllAssetCategoriesAsync();
            return _mapper.Map<List<AssetCategoryDetails>>(entities);
        }

        public async Task<AssetCategoryDetails?> GetByIdAsync(Guid id)
        {
            var entity = await _assetCategoryRepository.GetAssetCategoryByIdAsync(id);
            return entity != null ? _mapper.Map<AssetCategoryDetails>(entity) : null;
        }

        public async Task<AssetCategoryDetails> CreateAsync(AssetCategoryCreateRequest dto)
        {
            var nameExists = await _assetCategoryRepository.IsNameExistsAsync(dto.CategoryName.Trim());
            if (nameExists)
                throw new InvalidOperationException($"Category '{dto.CategoryName}' already exists.");

            var entity = _mapper.Map<AssetCategory>(dto);
            entity.CategoryName = dto.CategoryName.Trim();

            await _assetCategoryRepository.AddAssetCategoryAsync(entity);
            await _assetCategoryRepository.SaveChangesAsync();

            _logger.LogInformation("Category created: {Name}", entity.CategoryName);

            return _mapper.Map<AssetCategoryDetails>(entity);
        }

        public async Task<AssetCategoryDetails?> UpdateAsync(Guid id, AssetCategoryUpdateRequest dto)
        {
            var entity = await _assetCategoryRepository.GetAssetCategoryByIdAsync(id);
            if (entity == null) return null;

            var nameExists = await _assetCategoryRepository.IsNameExistsAsync(dto.CategoryName.Trim(), id);
            if (nameExists)
                throw new InvalidOperationException($"Category '{dto.CategoryName}' already exists.");

            entity.CategoryName = dto.CategoryName.Trim();
            entity.IsActive = dto.IsActive;

            _assetCategoryRepository.UpdateAssetCategory(entity);
            await _assetCategoryRepository.SaveChangesAsync();

            _logger.LogInformation("Category updated: {Id}", id);

            return _mapper.Map<AssetCategoryDetails>(entity);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var entity = await _assetCategoryRepository.GetAssetCategoryByIdAsync(id);
            if (entity == null) return false;

            entity.IsDeleted = true;
            entity.IsActive = false;

            _assetCategoryRepository.UpdateAssetCategory(entity);
            await _assetCategoryRepository.SaveChangesAsync();

            _logger.LogInformation("Category soft-deleted: {Id}", id);

            return true;
        }
    }
    }
