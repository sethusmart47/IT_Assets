using ITAssetManagement.Data;
using ITAssetManagement.Domain.Entities;
using ITAssetManagement.Domain.Enums;
using ITAssetManagement.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace ITAssetManagement.Repositories.Implementations
{
    public class PurchaseRepository : Repository<Purchase>, IPurchaseRepository
    {
        public PurchaseRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<List<Purchase>> GetAllPurchasesAsync(PurchaseStatus? status = null)
        {
            var query = Context.Purchases
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
            return await Context.Purchases
                .Include(p => p.Vendor)
                .Where(p => !p.IsDeleted && p.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task<Purchase?> GetPurchaseByIdWithDetailsAsync(Guid id)
        {
            return await Context.Purchases
                .Include(p => p.Vendor)
                .Include(p => p.PurchasedItems.Where(i => !i.IsDeleted))
                .Include(p => p.Attachments.Where(a => !a.IsDeleted))
                .Where(p => !p.IsDeleted)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<List<Purchase>> GetConfirmedWithItemsAsync()
        {
            return await Context.Purchases
                .AsNoTracking()
                .Include(p => p.Vendor)
                .Include(p => p.PurchasedItems.Where(i => !i.IsDeleted))
                .Where(p => !p.IsDeleted && p.Status == PurchaseStatus.Confirmed)
                .ToListAsync();
        }

        public async Task<Purchase?> GetByIdWithItemsAsync(Guid id)
        {
            return await Context.Purchases
                .AsNoTracking()
                .Include(p => p.PurchasedItems.Where(i => !i.IsDeleted))
                .FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted);
        }

        public async Task<bool> IsInvoiceNumberExistsAsync(string invoiceNumber, Guid? excludeId = null)
        {
            return await Context.Purchases
                .AnyAsync(p => p.InvoiceNumber.ToLower() == invoiceNumber.ToLower()
                    && !p.IsDeleted
                    && (!excludeId.HasValue || p.Id != excludeId.Value));
        }

        public async Task<string> GetNextPurchaseNumberAsync()
        {
            var today = DateTime.UtcNow.ToString("yyyyMMdd");
            var prefix = $"PO-{today}-";

            var lastNumber = await Context.Purchases
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

        public async Task<bool> HasPurchasedItemsAsync(Guid purchaseId)
        {
            return await Context.PurchasedItems
                .AnyAsync(i => i.PurchaseId == purchaseId && !i.IsDeleted);
        }

        public async Task AddPurchaseAsync(Purchase entity)
        {
            await Context.Purchases.AddAsync(entity);
        }

        public void UpdatePurchase(Purchase entity)
        {
            Context.Purchases.Update(entity);
        }

        public override async Task<int> SaveChangesAsync()
        {
            return await Context.SaveChangesAsync();
        }
    }
}
