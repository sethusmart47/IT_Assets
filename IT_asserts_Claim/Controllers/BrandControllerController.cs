

using IT_asserts_Claim.Dtos.Asset_Brand;
using IT_asserts_Claim.Dtos.Asset_Category;
using IT_asserts_Claim.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IT_asserts_Claim.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BrandController : ControllerBase
    {
        private readonly IAssetBrandService _brandService;

        public BrandController(IAssetBrandService brandService)
        {
            _brandService = brandService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var result = await _brandService.GetAllAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            try
            {
                var result = await _brandService.GetByIdAsync(id);

                if (result == null)
                    return NotFound("Brand not found.");

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{categoryId:guid}")]
        public async Task<IActionResult> GetByCategoryId(Guid categoryId)
        {
            try
            {
                var result = await _brandService.GetByCategoryIdAsync(categoryId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateAssetBrandDto dto)
        {
            if (dto == null)
                return BadRequest("Request body is required.");
            if(dto.AssetCategoryId==null)
                return BadRequest("AssetCategoryId is required.");

            try
            {
                var result = await _brandService.CreateAsync(dto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateAssetBrandDto dto)
        {
            if (dto == null)
                return BadRequest("Request body is required.");

            try
            {
                var result = await _brandService.UpdateAsync(id, dto);

                if (result == null)
                    return NotFound("Brand not found.");

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var result = await _brandService.DeleteAsync(id);

                if (!result)
                    return NotFound("Brand not found.");

                return Ok("Deleted successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
