using ITAssetManagement.Data;
using ITAssetManagement.Domain.Entities;
using ITAssetManagement.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace ITAssetManagement.Repositories.Implementations
{
    public class PurchaseAttachmentRepository : IPurchaseAttachmentRepository
    {
        private readonly AppDbContext _context;

        public PurchaseAttachmentRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<PurchaseAttachment>> GetAllByPurchaseIdAsync(Guid purchaseId)
        {
            return await _context.PurchaseAttachments
                .Where(a => a.PurchaseId == purchaseId && !a.IsDeleted)
                .OrderByDescending(a => a.CreatedAt)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<PurchaseAttachment?> GetPurchaseAttachmentByIdAsync(Guid attachmentId)
        {
            return await _context.PurchaseAttachments
                .Where(a => !a.IsDeleted && a.Id == attachmentId)
                .FirstOrDefaultAsync();
        }

        public async Task AddPurchaseAttachmentAsync(PurchaseAttachment entity)
        {
            await _context.PurchaseAttachments.AddAsync(entity);
        }

        public async Task AddPurchaseAttachmentsAsync(List<PurchaseAttachment> entities)
        {
            await _context.PurchaseAttachments.AddRangeAsync(entities);
        }

        public void UpdatePurchaseAttachment(PurchaseAttachment entity)
        {
            _context.PurchaseAttachments.Update(entity);
        }

        // Generic wrapper for backward compatibility
        public async Task<PurchaseAttachment?> GetByIdAsync(Guid attachmentId)
            => await GetPurchaseAttachmentByIdAsync(attachmentId);
        public async Task AddAsync(PurchaseAttachment entity)
            => await AddPurchaseAttachmentAsync(entity);
        public async Task AddRangeAsync(List<PurchaseAttachment> entities)
            => await AddPurchaseAttachmentsAsync(entities);
        public void Update(PurchaseAttachment entity)
            => UpdatePurchaseAttachment(entity);
        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}