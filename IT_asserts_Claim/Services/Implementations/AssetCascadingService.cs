using AutoMapper;
using ITAssetManagement.Repositories.Interface;
using ITAssetManagement.Services.Interface;
using static ITAssetManagement.Dtos.AssetCascading;

namespace ITAssetManagement.Services.Implementations;

public class AssetCascadingService : IAssetCascadingService
{
    private readonly IAssetCategoryRepository _categoryRepository;
    private readonly IAssetBrandRepository _brandRepository;
    private readonly IAssetModelRepository _modelRepository;
    private readonly IMapper _mapper;

    public AssetCascadingService(
        IAssetCategoryRepository categoryRepository,
        IAssetBrandRepository brandRepository,
        IAssetModelRepository modelRepository,
        IMapper mapper)
    {
        _categoryRepository = categoryRepository;
        _brandRepository = brandRepository;
        _modelRepository = modelRepository;
        _mapper = mapper;
    }

    public async Task<AssetCascadingDropdownDto> GetCascadingAsync()
    {
        var categories = await _categoryRepository.GetAllAssetCategoriesAsync();
        var brands = await _brandRepository.GetAllAssetBrandsAsync();
        var models = await _modelRepository.GetAllAssetModelsAsync();

        return new AssetCascadingDropdownDto
        {
            Categories = _mapper.Map<List<CategoryDropdownItem>>(categories.Where(c => c.IsActive).OrderBy(c => c.CategoryName).ToList()),
            Brands = _mapper.Map<List<BrandDropdownItem>>(brands.Where(b => b.IsActive).OrderBy(b => b.BrandName).ToList()),
            Models = _mapper.Map<List<ModelDropdownItem>>(models.Where(m => m.IsActive).OrderBy(m => m.ModelName).ToList())
        };
    }
}
