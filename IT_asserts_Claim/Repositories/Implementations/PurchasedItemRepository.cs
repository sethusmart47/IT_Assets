using IT_asserts_Claim.Data;
using IT_asserts_Claim.entity;
using IT_asserts_Claim.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace IT_asserts_Claim.Repositories.Implementations
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

        public async Task<PurchasedItem?> GetByIdAsync(Guid itemId)
        {
            return await _context.PurchasedItems
                .Where(i => !i.IsDeleted && i.Id == itemId)
                .FirstOrDefaultAsync();
        }

        public async Task AddAsync(PurchasedItem entity)
        {
            await _context.PurchasedItems.AddAsync(entity);
        }

        public void Update(PurchasedItem entity)
        {
            _context.PurchasedItems.Update(entity);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
