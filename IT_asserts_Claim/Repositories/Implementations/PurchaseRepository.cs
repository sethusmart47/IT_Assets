using ITAssetManagement.Data;
using ITAssetManagement.Domain.Entities;
using ITAssetManagement.Domain.Enums;
using ITAssetManagement.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace ITAssetManagement.Repositories.Implementations
{
    public class PurchaseRepository : IPurchaseRepository
    {
        private readonly AppDbContext _context;

        public PurchaseRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Purchase>> GetAllPurchasesAsync(PurchaseStatus? status = null)
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

        public async Task<Purchase?> GetPurchaseByIdAsync(Guid id)
        {
            return await _context.Purchases
                .Include(p => p.Vendor)
                .Where(p => !p.IsDeleted && p.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task<Purchase?> GetPurchaseByIdWithDetailsAsync(Guid id)
        {
            return await _context.Purchases
                .Include(p => p.Vendor)
                .Include(p => p.PurchasedItems.Where(i => !i.IsDeleted))
                .Include(p => p.Attachments.Where(a => !a.IsDeleted))
                .Where(p => !p.IsDeleted)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<List<Purchase>> GetCompletedWithItemsAsync()
        {
            return await _context.Purchases
                .AsNoTracking()
                .Include(p => p.Vendor)
                .Include(p => p.PurchasedItems.Where(i => !i.IsDeleted))
                .Where(p => !p.IsDeleted && p.Status == PurchaseStatus.Completed)
                .ToListAsync();
        }

        public async Task<Purchase?> GetByIdWithItemsAsync(Guid id)
        {
            return await _context.Purchases
                .AsNoTracking()
                .Include(p => p.PurchasedItems.Where(i => !i.IsDeleted))
                .FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted);
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

        public async Task AddPurchaseAsync(Purchase entity)
        {
            await _context.Purchases.AddAsync(entity);
        }

        public void UpdatePurchase(Purchase entity)
        {
            _context.Purchases.Update(entity);
        }

        // Generic wrappers for backward compatibility
        public async Task<List<Purchase>> GetAllAsync(PurchaseStatus? status = null)
            => await GetAllPurchasesAsync(status);

        public async Task<Purchase?> GetByIdAsync(Guid id)
            => await GetPurchaseByIdAsync(id);

        public async Task<Purchase?> GetByIdWithDetailsAsync(Guid id)
            => await GetPurchaseByIdWithDetailsAsync(id);

        public async Task AddAsync(Purchase entity)
            => await AddPurchaseAsync(entity);

        public void Update(Purchase entity)
            => UpdatePurchase(entity);

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}