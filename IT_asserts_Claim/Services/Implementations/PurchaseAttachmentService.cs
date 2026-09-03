using AutoMapper;
using ITAssetManagement.Dtos.Purchase;
using ITAssetManagement.Domain.Entities;
using ITAssetManagement.Repositories.Interface;
using ITAssetManagement.Services.Interface;

namespace ITAssetManagement.Services.Implementations
{
    public class PurchaseAttachmentService : IPurchaseAttachmentService
    {
        private readonly IPurchaseAttachmentRepository _attachmentRepository;
        private readonly IPurchaseRepository _purchaseRepository;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _environment;
        private readonly ILogger<PurchaseAttachmentService> _logger;

        private static readonly string[] AllowedContentTypes =
        {
        "image/jpeg", "image/jpg", "image/png", "application/pdf"
    };
        private const long MaxFileSize = 25 * 1024 * 1024; // 25 MB
        private const int MaxFilesPerUpload = 10;

        public PurchaseAttachmentService(
            IPurchaseAttachmentRepository attachmentRepository,
            IPurchaseRepository purchaseRepository,
            IMapper mapper,
            IWebHostEnvironment environment,
            ILogger<PurchaseAttachmentService> logger)
        {
            _attachmentRepository = attachmentRepository;
            _purchaseRepository = purchaseRepository;
            _mapper = mapper;
            _environment = environment;
            _logger = logger;
        }

        public async Task<List<PurchaseAttachmentDetails>> GetAllByPurchaseIdAsync(Guid purchaseId)
        {
            var attachments = await _attachmentRepository.GetAllByPurchaseIdAsync(purchaseId);
            return _mapper.Map<List<PurchaseAttachmentDetails>>(attachments);
        }

        public async Task<List<PurchaseAttachmentDetails>> UploadAsync(Guid purchaseId, List<IFormFile> files)
        {
            // Validate purchase exists
            var purchase = await _purchaseRepository.GetPurchaseByIdAsync(purchaseId);
            if (purchase == null)
                throw new InvalidOperationException("Purchase not found.");

            if (files == null || files.Count == 0)
                throw new InvalidOperationException("No files provided.");

            if (files.Count > MaxFilesPerUpload)
                throw new InvalidOperationException($"Maximum {MaxFilesPerUpload} files allowed per upload.");

            // Build upload folder path
            var relativeFolderPath = Path.Combine("uploads", "purchases", purchaseId.ToString());
            var absoluteFolderPath = Path.Combine(_environment.WebRootPath, relativeFolderPath);

            if (!Directory.Exists(absoluteFolderPath))
                Directory.CreateDirectory(absoluteFolderPath);

            var attachments = new List<PurchaseAttachment>();

            foreach (var file in files)
            {
                // Validate each file
                ValidateFile(file);

                // Generate unique file name to prevent overwrite
                var uniqueFileName = $"{Guid.NewGuid().ToString("N")[..8]}_{file.FileName}";
                var absoluteFilePath = Path.Combine(absoluteFolderPath, uniqueFileName);
                var relativeFilePath = $"/{relativeFolderPath.Replace("\\", "/")}/{uniqueFileName}";

                // Save file to local folder
                using (var stream = new FileStream(absoluteFilePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                // Create entity
                var attachment = new PurchaseAttachment
                {
                    Id = Guid.NewGuid(),
                    PurchaseId = purchaseId,
                    FileName = file.FileName,
                    FilePath = relativeFilePath,
                    ContentType = file.ContentType,
                    FileSize = file.Length
                };

                attachments.Add(attachment);
            }

            await _attachmentRepository.AddPurchaseAttachmentsAsync(attachments);
            await _attachmentRepository.SaveChangesAsync();

            _logger.LogInformation("{Count} file(s) uploaded for Purchase: {PurchaseId}",
                attachments.Count, purchaseId);

            return _mapper.Map<List<PurchaseAttachmentDetails>>(attachments);
        }

        public async Task<(byte[]? FileData, string? ContentType, string? FileName)> DownloadAsync(Guid attachmentId)
        {
            var attachment = await _attachmentRepository.GetPurchaseAttachmentByIdAsync(attachmentId);
            if (attachment == null)
                return (null, null, null);

            // Build absolute path from relative stored path
            var absolutePath = Path.Combine(_environment.WebRootPath, attachment.FilePath.TrimStart('/'));

            if (!File.Exists(absolutePath))
                return (null, null, null);

            var fileData = await File.ReadAllBytesAsync(absolutePath);
            return (fileData, attachment.ContentType, attachment.FileName);
        }

        public async Task<bool> DeleteAsync(Guid attachmentId)
        {
            var attachment = await _attachmentRepository.GetPurchaseAttachmentByIdAsync(attachmentId);
            if (attachment == null) return false;

            // Soft delete in DB (file stays on disk — cleanup via scheduled job if needed)
            attachment.IsDeleted = true;
            attachment.IsActive = false;

            _attachmentRepository.Update(attachment);
            await _attachmentRepository.SaveChangesAsync();

            _logger.LogInformation("Attachment soft-deleted: {AttachmentId}, File: {FileName}",
                attachmentId, attachment.FileName);

            return true;
        }

        // ─── PRIVATE HELPER ───

        private static void ValidateFile(IFormFile file)
        {
            if (file.Length == 0)
                throw new InvalidOperationException($"File '{file.FileName}' is empty.");

            if (file.Length > MaxFileSize)
                throw new InvalidOperationException(
                    $"File '{file.FileName}' exceeds maximum size of 25 MB. Current size: {file.Length / 1_048_576.0:F1} MB.");

            if (!AllowedContentTypes.Contains(file.ContentType.ToLower()))
                throw new InvalidOperationException(
                    $"File '{file.FileName}' has unsupported type '{file.ContentType}'. Allowed: PDF, PNG, JPG, JPEG.");
        }
    }
}