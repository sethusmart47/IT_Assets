using ITAssetManagement.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace ITAssetManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssetCascadingController : ControllerBase
    {
        private readonly IAssetCascadingService _cascadingService;

        public AssetCascadingController(IAssetCascadingService cascadingService)
        {
            _cascadingService = cascadingService;
        }

        /// <summary>
        /// Returns all Categories, Brands, Models in ONE call.
        /// Frontend filters locally for cascading — no extra API calls per dropdown change.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetCascadingDropdown()
        {
            var result = await _cascadingService.GetCascadingAsync();
            return Ok(result);
        }
    }
}
