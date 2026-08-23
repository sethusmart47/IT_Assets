using IT_asserts_Claim.Data;
using IT_asserts_Claim.entity;
using IT_asserts_Claim.Enum;
using IT_asserts_Claim.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace IT_asserts_Claim.Repositories.Implementations
{
    public class PurchaseRepository : IPurchaseRepository
    {
        private readonly AppDbContext _context;

        public PurchaseRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Purchase>> GetAllAsync(PurchaseStatus? status = null)
        {
            var query = _context.Purchases
                .Include(p => p.Vendor)
                .Include(p => p.PurchasedItems.Where(i => !i.IsDeleted))
                .Include(p => p.Attachments.Where(a => !a.IsDeleted))
                .Where(p => !p.IsDeleted);


            if (status.HasValue)
                query = query.Where(p => p.Status == status.Value);

            return await query
                .OrderByDescending(p => p.CreatedAt)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Purchase?> GetByIdAsync(Guid id)
        {
            return await _context.Purchases
                .Include(p => p.Vendor)
                .Where(p => !p.IsDeleted && p.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task<Purchase?> GetByIdWithDetailsAsync(Guid id)
        {
            return await _context.Purchases
                .Include(p => p.Vendor)
                .Include(p => p.PurchasedItems.Where(i => !i.IsDeleted))
                .Include(p => p.Attachments.Where(a => !a.IsDeleted))
                .Where(p => !p.IsDeleted)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<bool> IsInvoiceNumberExistsAsync(string invoiceNumber, Guid? excludeId = null)
        {
            return await _context.Purchases
                .AnyAsync(p => p.InvoiceNumber.ToLower() == invoiceNumber.ToLower()
                    && !p.IsDeleted
                    && (!excludeId.HasValue || p.Id != excludeId.Value));
        }

        public async Task<string> GetNextPurchaseNumberAsync()
        {
            var today = DateTime.UtcNow.ToString("yyyyMMdd");
            var prefix = $"PO-{today}-";

            var lastNumber = await _context.Purchases
                .Where(p => p.PurchaseNumber.StartsWith(prefix))
                .OrderByDescending(p => p.PurchaseNumber)
                .Select(p => p.PurchaseNumber)
                .FirstOrDefaultAsync();

            if (lastNumber == null)
                return $"{prefix}001";

            var lastSequence = lastNumber.Replace(prefix, "");
            if (int.TryParse(lastSequence, out int seq))
                return $"{prefix}{(seq + 1).ToString("D3")}";

            return $"{prefix}001";
        }

        public async Task AddAsync(Purchase entity)
        {
            await _context.Purchases.AddAsync(entity);
        }

        public void Update(Purchase entity)
        {
            _context.Purchases.Update(entity);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}