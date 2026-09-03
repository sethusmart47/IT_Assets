using ITAssetManagement.Data;
using ITAssetManagement.Domain.Entities;
using ITAssetManagement.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace ITAssetManagement.Repositories.Implementations
{
    public class PurchaseAttachmentRepository : Repository<PurchaseAttachment>, IPurchaseAttachmentRepository
    {
        public PurchaseAttachmentRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<List<PurchaseAttachment>> GetAllByPurchaseIdAsync(Guid purchaseId)
        {
            return await Context.PurchaseAttachments
                .Where(a => a.PurchaseId == purchaseId && !a.IsDeleted)
                .OrderByDescending(a => a.CreatedAt)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<PurchaseAttachment?> GetPurchaseAttachmentByIdAsync(Guid attachmentId)
        {
            return await Context.PurchaseAttachments
                .Where(a => !a.IsDeleted && a.Id == attachmentId)
                .FirstOrDefaultAsync();
        }

        public async Task AddPurchaseAttachmentAsync(PurchaseAttachment entity)
        {
            await Context.PurchaseAttachments.AddAsync(entity);
        }

        public async Task AddPurchaseAttachmentsAsync(List<PurchaseAttachment> entities)
        {
            await Context.PurchaseAttachments.AddRangeAsync(entities);
        }

        public void UpdatePurchaseAttachment(PurchaseAttachment entity)
        {
            Context.PurchaseAttachments.Update(entity);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await Context.SaveChangesAsync();
        }
    }
}
