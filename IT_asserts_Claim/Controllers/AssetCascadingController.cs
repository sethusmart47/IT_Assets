using AutoMapper;
using IT_asserts_Claim.Repositories.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static IT_asserts_Claim.Dtos.AssetCascading;

namespace IT_asserts_Claim.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssetCascadingController : ControllerBase
    {
        private readonly IAssetCategoryRepository _categoryRepository;
        private readonly IAssetBrandRepository _brandRepository;
        private readonly IAssetModelRepository _modelRepository;
        private readonly IMapper _mapper;

        public AssetCascadingController(
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

        /// <summary>
        /// Returns all Categories, Brands, Models in ONE call.
        /// Frontend filters locally for cascading — no extra API calls per dropdown change.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetCascadingDropdown()
        {
            try
            {
                var categories = await _categoryRepository.GetAllAsync();
                var brands = await _brandRepository.GetAllAsync();
                var models = await _modelRepository.GetAllAsync();

                var result = new AssetCascadingDropdownDto
                {
                    Categories = _mapper.Map<List<CategoryDropdownItem>>(
                        categories.Where(c => c.IsActive).OrderBy(c => c.CategoryName).ToList()),

                    Brands = _mapper.Map<List<BrandDropdownItem>>(
                        brands.Where(b => b.IsActive).OrderBy(b => b.BrandName).ToList()),

                    Models = _mapper.Map<List<ModelDropdownItem>>(
                        models.Where(m => m.IsActive).OrderBy(m => m.ModelName).ToList())
                };

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
