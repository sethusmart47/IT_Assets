using ITAssetManagement.Data;
using ITAssetManagement.Domain.Entities;
using ITAssetManagement.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace ITAssetManagement.Repositories.Implementations
{
    public class PurchasedItemRepository : Repository<PurchasedItem>, IPurchasedItemRepository
    {
        public PurchasedItemRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<List<PurchasedItem>> GetAllByPurchaseIdAsync(Guid purchaseId)
        {
            return await Context.PurchasedItems
                .Where(i => i.PurchaseId == purchaseId && !i.IsDeleted)
                .OrderBy(i => i.CreatedAt)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<PurchasedItem?> GetPurchasedItemByIdAsync(Guid itemId)
        {
            return await Context.PurchasedItems
                .Where(i => !i.IsDeleted && i.Id == itemId)
                .FirstOrDefaultAsync();
        }

        public async Task AddPurchasedItemAsync(PurchasedItem entity)
        {
            await Context.PurchasedItems.AddAsync(entity);
        }

        public void UpdatePurchasedItem(PurchasedItem entity)
        {
            Context.PurchasedItems.Update(entity);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await Context.SaveChangesAsync();
        }
    }
}
