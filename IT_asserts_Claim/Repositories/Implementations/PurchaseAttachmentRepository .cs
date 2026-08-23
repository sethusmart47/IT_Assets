using IT_asserts_Claim.Data;
using IT_asserts_Claim.entity;
using IT_asserts_Claim.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace IT_asserts_Claim.Repositories.Implementations
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

        public async Task<PurchaseAttachment?> GetByIdAsync(Guid attachmentId)
        {
            return await _context.PurchaseAttachments
                .Where(a => !a.IsDeleted && a.Id == attachmentId)
                .FirstOrDefaultAsync();
        }

        public async Task AddAsync(PurchaseAttachment entity)
        {
            await _context.PurchaseAttachments.AddAsync(entity);
        }

        public async Task AddRangeAsync(List<PurchaseAttachment> entities)
        {
            await _context.PurchaseAttachments.AddRangeAsync(entities);
        }

        public void Update(PurchaseAttachment entity)
        {
            _context.PurchaseAttachments.Update(entity);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}