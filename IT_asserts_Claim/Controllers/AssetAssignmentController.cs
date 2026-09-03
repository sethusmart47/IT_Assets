using ITAssetManagement.Dtos.AssetAssignment;
using ITAssetManagement.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ITAssetManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssetAssignmentController : ControllerBase
    {
        private readonly IAssetAssignmentService _assetAssignmentService;

        public AssetAssignmentController(IAssetAssignmentService assetAssignmentService)
        {
            _assetAssignmentService = assetAssignmentService;
        }

        //// GET: api/asset-assignment/employees
        //[HttpGet("employees")]
        //public async Task<IActionResult> GetAllEmployees()
        //{
        //    try
        //    {
        //        var employees = await _assetAssignmentService.GetAllEmpAsync();
        //        return Ok(employees);
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, ex.Message);
        //    }
        //}

        // GET: api/asset-assignment/employees/{employeeId}/assignments
        [HttpGet("employees/{employeeId}/assignments")]
        public async Task<IActionResult> GetAssignmentsByEmployee(Guid employeeId)
        {
            try
            {
                var assignments = await _assetAssignmentService.GetAssignmentsByEmployeeIdAsync(employeeId);
                return Ok(assignments);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // POST: api/asset-assignment/assign
        [HttpPost("assign")]
        public async Task<IActionResult> AssignAsset([FromBody] AssignAssetDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var result = await _assetAssignmentService.AssignAssetAsync(dto);
                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
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

        // POST: api/asset-assignment/{assignmentId}/return
        [HttpPost("{assignmentId}/return")]
        public async Task<IActionResult> ReturnAsset(Guid assignmentId, [FromBody] ReturnAssetDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var result = await _assetAssignmentService.ReturnAssetAsync(assignmentId, dto);
                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
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

        // POST: api/asset-assignment/surrender/{employeeId}
        [HttpPost("surrender/{employeeId}")]
        public async Task<IActionResult> SurrenderAssets(Guid employeeId, [FromBody] SurrenderAssetsDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var results = await _assetAssignmentService.SurrenderAssetsAsync(employeeId, dto);
                return Ok(results);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
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
    }
}

