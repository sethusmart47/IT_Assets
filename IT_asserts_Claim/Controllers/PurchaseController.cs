using ITAssetManagement.Dtos.Purchase;
using ITAssetManagement.Domain.Enums;
using ITAssetManagement.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace ITAssetManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PurchaseController : ControllerBase
    {
        private readonly IPurchaseService _purchaseService;

        public PurchaseController(IPurchaseService purchaseService)
        {
            _purchaseService = purchaseService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] PurchaseStatus? status = null)
        {
            var result = await _purchaseService.GetAllPurchasesAsync(status);
            return Ok(result);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _purchaseService.GetPurchaseDetailsAsync(id);
            if (result == null)
                return NotFound(new { message = "Purchase not found." });
            return Ok(result);
        }

        [HttpGet("next-number")]
        public async Task<IActionResult> GetNextNumber()
        {
            var result = await _purchaseService.GetNextPurchaseNumberAsync();
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] PurchaseCreateRequest dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
                var result = await _purchaseService.CreatePurchaseAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] PurchaseUpdateRequest dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
                var result = await _purchaseService.UpdatePurchaseAsync(id, dto);
                if (result == null)
                    return NotFound(new { message = "Purchase not found." });
                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id:guid}/receive")]
        [HttpPut("{id:guid}/complete")]
        public async Task<IActionResult> Receive(Guid id)
        {
            try
            {
                var result = await _purchaseService.ReceivePurchaseAsync(id);
                if (!result)
                    return NotFound(new { message = "Purchase not found." });
                return Ok(new { message = "Purchase received successfully." });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var result = await _purchaseService.DeletePurchaseAsync(id);
                if (!result)
                    return NotFound(new { message = "Purchase not found." });
                return Ok(new { message = "Purchase deleted successfully." });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
