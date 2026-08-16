using IT_asserts_Claim.Dtos.Asset_Category;
using IT_asserts_Claim.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IT_asserts_Claim.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly IAssetCategoryService _categoryService;

        public CategoryController(IAssetCategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var result = await _categoryService.GetAllAsync();
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
                var result = await _categoryService.GetByIdAsync(id);

                if (result == null)
                    return NotFound("Category not found.");

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create(  [FromBody] CreateAssetCategoryDto dto)
        {
            if (dto == null)
                return BadRequest("Request body is required.");

            try
            {
                var result = await _categoryService.CreateAsync(dto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateAssetCategoryDto dto)
        {
            if (dto == null)
                return BadRequest("Request body is required.");

            try
            {
                var result = await _categoryService.UpdateAsync(id, dto);

                if (result == null)
                    return NotFound("Category not found.");

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
                var result = await _categoryService.DeleteAsync(id);

                if (!result)
                    return NotFound("Category not found.");

                return Ok("Deleted successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
