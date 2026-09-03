using ITAssetManagement.Data;
using ITAssetManagement.Domain.Entities;
using ITAssetManagement.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace ITAssetManagement.Repositories.Implementations
{
    public class PurchasedItemRepository : IPurchasedItemRepository
    {
        private readonly AppDbContext _context;

        public PurchasedItemRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<PurchasedItem>> GetAllByPurchaseIdAsync(Guid purchaseId)
        {
            return await _context.PurchasedItems
                .Where(i => i.PurchaseId == purchaseId && !i.IsDeleted)
                .OrderBy(i => i.CreatedAt)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<PurchasedItem?> GetPurchasedItemByIdAsync(Guid itemId)
        {
            return await _context.PurchasedItems
                .Where(i => !i.IsDeleted && i.Id == itemId)
                .FirstOrDefaultAsync();
        }

        public async Task AddPurchasedItemAsync(PurchasedItem entity)
        {
            await _context.PurchasedItems.AddAsync(entity);
        }

        public void UpdatePurchasedItem(PurchasedItem entity)
        {
            _context.PurchasedItems.Update(entity);
        }

        // Generic wrapper for backward compatibility
        public async Task<PurchasedItem?> GetByIdAsync(Guid itemId)
            => await GetPurchasedItemByIdAsync(itemId);
        public async Task AddAsync(PurchasedItem entity)
            => await AddPurchasedItemAsync(entity);
        public void Update(PurchasedItem entity)
            => UpdatePurchasedItem(entity);
        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
