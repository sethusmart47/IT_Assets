using ITAssetManagement.Dtos.PurchaseItem;
using ITAssetManagement.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ITAssetManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PurchasedItemController : ControllerBase
    {
       
            private readonly IPurchasedItemService _itemService;

            public PurchasedItemController(IPurchasedItemService itemService)
            {
                _itemService = itemService;
            }

            /// <summary>
            /// Get all items for a specific purchase.
            /// </summary>
            [HttpGet]
            public async Task<IActionResult> GetAll(Guid purchaseId)
            {
                try
                {
                    var result = await _itemService.GetAllByPurchaseIdAsync(purchaseId);
                    return Ok(result);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }

            /// <summary>
            /// Get single item by Id.
            /// </summary>
            [HttpGet("{itemId:guid}")]
            public async Task<IActionResult> GetById(Guid purchaseId, Guid itemId)
            {
                try
                {
                    var result = await _itemService.GetByIdAsync(itemId);
                    if (result == null)
                        return NotFound("Item not found.");
                    return Ok(result);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }

            /// <summary>
            /// Add new item to purchase.
            /// </summary>
            [HttpPost("{purchaseId}")]
            public async Task<IActionResult> Create(Guid purchaseId, [FromBody] PurchasedItemCreateRequest dto)
            {
                try
                {
                    if (!ModelState.IsValid)
                        return BadRequest(ModelState);

                    var result = await _itemService.CreateAsync(purchaseId, dto);
                    return Ok(result);
                }
                catch (InvalidOperationException ex)
                {
                    return BadRequest(ex.Message);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }

            /// <summary>
            /// Update existing item.
            /// </summary>
            [HttpPut("{itemId}")]
            public async Task<IActionResult> Update(Guid itemId, [FromBody] PurchasedItemUpdateRequest dto)
            {
                try
                {
                    if (!ModelState.IsValid)
                        return BadRequest(ModelState);

                    var result = await _itemService.UpdateAsync(itemId, dto);
                    if (result == null)
                        return NotFound("Item not found.");
                    return Ok(result);
                }
                catch (InvalidOperationException ex)
                {
                    return BadRequest(ex.Message);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }

            /// <summary>
            /// Soft delete item (recalculates purchase total).
            /// </summary>
            [HttpDelete("{itemId:guid}")]
            public async Task<IActionResult> Delete(Guid purchaseId, Guid itemId)
            {
                try
                {
                    var result = await _itemService.DeleteAsync(itemId);
                    if (!result)
                        return NotFound("Item not found.");
                    return Ok("Item deleted successfully.");
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
        }
    
}