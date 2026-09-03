using ITAssetManagement.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ITAssetManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PurchaseAttachmentController : ControllerBase
    {
        private readonly IPurchaseAttachmentService _attachmentService;

        public PurchaseAttachmentController(IPurchaseAttachmentService attachmentService)
        {
            _attachmentService = attachmentService;
        }

        /// <summary>
        /// Get all attachments for a specific purchase.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAll(Guid purchaseId)
        {
            try
            {
                var result = await _attachmentService.GetAllByPurchaseIdAsync(purchaseId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Upload one or more files (PDF, PNG, JPG, JPEG — max 25 MB each, max 10 files).
        /// </summary>
        [HttpPost("{purchaseId}/attachments")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Upload(Guid purchaseId, [FromForm] List<IFormFile> files)
        {
            try
            {
                var result = await _attachmentService.UploadAsync(purchaseId, files);
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
        /// Download/view a specific attachment file.
        /// </summary>
        [HttpGet("{attachmentId:guid}/download")]
        public async Task<IActionResult> Download(Guid purchaseId, Guid attachmentId)
        {
            try
            {
                var (fileData, contentType, fileName) = await _attachmentService.DownloadAsync(attachmentId);

                if (fileData == null)
                    return NotFound("Attachment not found or file missing from storage.");

                return File(fileData, contentType ?? "application/octet-stream", fileName ?? "download");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("{purchaseId:guid}/{attachmentId:guid}/preview")]
        public async Task<IActionResult> Preview(Guid purchaseId, Guid attachmentId)
        {
            try
            {
                var (fileData, contentType, fileName) = await _attachmentService.DownloadAsync(attachmentId);

                if (fileData == null)
                    return NotFound("Attachment not found or file missing from storage.");

                // KEY: Return File WITHOUT fileName parameter
                // This sets Content-Disposition: inline (browser renders it)
                // vs File(data, type, "name.jpg") which sets Content-Disposition: attachment (download)
                return File(fileData, contentType ?? "application/octet-stream");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// Soft delete an attachment.
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var result = await _attachmentService.DeleteAsync(id);
                if (!result)
                    return NotFound("Attachment not found.");
                return Ok("Attachment deleted successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
