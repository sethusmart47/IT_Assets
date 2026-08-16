using AutoMapper;
using IT_asserts_Claim.Dtos;
using IT_asserts_Claim.Dtos.Asset_Category;
using IT_asserts_Claim.DTOs;
using IT_asserts_Claim.entity;
using IT_asserts_Claim.Repositories.Interface;
using IT_asserts_Claim.Services.Interface;
using Microsoft.Extensions.Logging;

namespace IT_asserts_Claim.Services.Implementations
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
        public async Task<List<AssetCategoryDto>> GetAllAsync()
        {
            var entities = await _assetCategoryRepository.GetAllAsync();
            return _mapper.Map<List<AssetCategoryDto>>(entities);
        }

        public async Task<AssetCategoryDto?> GetByIdAsync(Guid id)
        {
            var entity = await _assetCategoryRepository.GetByIdAsync(id);
            return entity != null ? _mapper.Map<AssetCategoryDto>(entity) : null;
        }

        public async Task<AssetCategoryDto> CreateAsync(Dtos.Asset_Category.CreateAssetCategoryDto dto)
        {
            var nameExists = await _assetCategoryRepository.IsNameExistsAsync(dto.CategoryName.Trim());
            if (nameExists)
                throw new InvalidOperationException($"Category '{dto.CategoryName}' already exists.");

            var entity = _mapper.Map<AssetCategory>(dto);
            entity.CategoryName = dto.CategoryName.Trim();

            await _assetCategoryRepository.AddAsync(entity);
            await _assetCategoryRepository.SaveChangesAsync();

            _logger.LogInformation("Category created: {Name}", entity.CategoryName);

            return _mapper.Map<AssetCategoryDto>(entity);
        }

        public async Task<AssetCategoryDto?> UpdateAsync(Guid id, Dtos.Asset_Category.UpdateAssetCategoryDto dto)
        {
            var entity = await _assetCategoryRepository.GetByIdAsync(id);
            if (entity == null) return null;

            var nameExists = await _assetCategoryRepository.IsNameExistsAsync(dto.CategoryName.Trim(), id);
            if (nameExists)
                throw new InvalidOperationException($"Category '{dto.CategoryName}' already exists.");

            entity.CategoryName = dto.CategoryName.Trim();
            entity.IsActive = dto.IsActive;

            _assetCategoryRepository.Update(entity);
            await _assetCategoryRepository.SaveChangesAsync();

            _logger.LogInformation("Category updated: {Id}", id);

            return _mapper.Map<AssetCategoryDto>(entity);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var entity = await _assetCategoryRepository.GetByIdAsync(id);
            if (entity == null) return false;

            entity.IsDeleted = true;
            entity.IsActive = false;

            _assetCategoryRepository.Update(entity);
            await _assetCategoryRepository.SaveChangesAsync();

            _logger.LogInformation("Category soft-deleted: {Id}", id);

            return true;
        }
    }
    }
