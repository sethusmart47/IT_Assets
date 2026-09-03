using ITAssetManagement.Dtos.Asset;
using ITAssetManagement.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ITAssetManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssetController : ControllerBase
    {
        private readonly IAssetService _assetService;

        public AssetController(IAssetService assetService)
        {
            _assetService = assetService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] string? status,
            [FromQuery] string? category,
            [FromQuery] string? brand,
            [FromQuery] string? search)
        {
            try
            {
                var assets = await _assetService.GetAllAsync(status, category, brand, search);
                return Ok(assets);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            try
            {
                var asset = await _assetService.GetByIdAsync(id);
                if (asset == null) return NotFound("Asset not found.");
                return Ok(asset);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] AssetCreateRequest dto)
        {
            try
            {
                var asset = await _assetService.RegisterAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = asset.Id }, asset);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("bulk-register")]
        public async Task<IActionResult> BulkRegister([FromBody] AssetBulkCreateRequest dto)
        {
            try
            {
                var assets = await _assetService.BulkRegisterAsync(dto);
                return Ok(assets);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] AssetUpdateRequest dto)
        {
            try
            {
                var asset = await _assetService.UpdateAsync(id, dto);
                if (asset == null) return NotFound("Asset not found.");
                return Ok(asset);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var result = await _assetService.DeleteAsync(id);
                if (!result) return NotFound("Asset not found.");
                return Ok("Asset deleted successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("validate-serials")]
        public async Task<IActionResult> ValidateSerialNumbers([FromBody] AssetValidateSerialNumbersRequest dto)
        {
            try
            {
                var result = await _assetService.ValidateSerialNumbersAsync(dto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("available-purchases")]
        public async Task<IActionResult> GetAvailablePurchases()
        {
            try
            {
                var purchases = await _assetService.GetAvailablePurchasesAsync();
                return Ok(purchases);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("available-items/{purchaseId}")]
        public async Task<IActionResult> GetAvailableItems(Guid purchaseId)
        {
            try
            {
                var items = await _assetService.GetAvailableItemsAsync(purchaseId);
                return Ok(items);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("registration-summary/{purchaseId}")]
        public async Task<IActionResult> GetRegistrationSummary(Guid purchaseId)
        {
            try
            {
                var summary = await _assetService.GetRegistrationSummaryAsync(purchaseId);
                if (summary == null) return NotFound("Purchase not found.");
                return Ok(summary);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpGet("available/search")]
        public async Task<IActionResult> SearchAvailableAsset(
             [FromQuery] string? serialNumber,
             [FromQuery] string? assetTag)
        {
            try
            {
                var result = await _assetService.SearchAvailableAssetAsync(serialNumber, assetTag);
                if (result == null) return NotFound("No available asset found matching the criteria.");
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}

