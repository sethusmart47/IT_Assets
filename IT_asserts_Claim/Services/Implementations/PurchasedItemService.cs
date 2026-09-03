using AutoMapper;
using ITAssetManagement.Dtos.PurchaseItem;
using ITAssetManagement.Domain.Entities;
using ITAssetManagement.Repositories.Interface;
using ITAssetManagement.Services.Interface;

namespace ITAssetManagement.Services.Implementations
{
    public class PurchasedItemService : IPurchasedItemService
    {
        private readonly IPurchasedItemRepository _itemRepository;
        private readonly IPurchaseRepository _purchaseRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<PurchasedItemService> _logger;

        public PurchasedItemService(
            IPurchasedItemRepository itemRepository,
            IPurchaseRepository purchaseRepository,
            IMapper mapper,
            ILogger<PurchasedItemService> logger)
        {
            _itemRepository = itemRepository;
            _purchaseRepository = purchaseRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<List<PurchasedItemDetails>> GetAllByPurchaseIdAsync(Guid purchaseId)
        {
            var items = await _itemRepository.GetAllByPurchaseIdAsync(purchaseId);
            return _mapper.Map<List<PurchasedItemDetails>>(items);
        }

        public async Task<PurchasedItemDetails?> GetByIdAsync(Guid itemId)
        {
            var item = await _itemRepository.GetPurchasedItemByIdAsync(itemId);
            if (item == null) return null;
            return _mapper.Map<PurchasedItemDetails>(item);
        }

        public async Task<PurchasedItemDetails> CreateAsync(Guid purchaseId, PurchasedItemCreateRequest dto)
        {
            // Validate purchase exists
            var purchase = await _purchaseRepository.GetPurchaseByIdAsync(purchaseId);
            if (purchase == null)
                throw new InvalidOperationException("Purchase not found.");

            // Map DTO ? Entity
            var item = _mapper.Map<PurchasedItem>(dto);
            item.Id = Guid.NewGuid();
            item.PurchaseId = purchaseId;
            item.SubTotal = dto.Quantity * dto.UnitPrice;

            await _itemRepository.AddPurchasedItemAsync(item);
            purchase.TotalAmount = purchase.TotalAmount + item.SubTotal;
            await _itemRepository.SaveChangesAsync();

            _logger.LogInformation("Item added: {Category} | {Brand} | {Model} ? Purchase {PurchaseId}",
                dto.Category, dto.Brand, dto.Model, purchaseId);

            return _mapper.Map<PurchasedItemDetails>(item);
        }

        public async Task<PurchasedItemDetails?> UpdateAsync(Guid itemId, PurchasedItemUpdateRequest dto)
        {
            var item = await _itemRepository.GetPurchasedItemByIdAsync(itemId);
            if (item == null) return null;

            var oldSubTotal = item.SubTotal;

            // Update fields directly
            item.Category = dto.Category.Trim();
            item.Brand = dto.Brand.Trim();
            item.Model = dto.Model.Trim();
            item.Configuration = dto.Configuration.Trim();
            item.Quantity = dto.Quantity;
            item.UnitPrice = dto.UnitPrice;
            //item.WarrantyPeriod = dto.WarrantyPeriod;
            item.SubTotal = dto.Quantity * dto.UnitPrice;

            _itemRepository.Update(item);
            var delta = item.SubTotal - oldSubTotal;
            if (delta != 0)
            {
                var purchase = await _purchaseRepository.GetPurchaseByIdAsync(item.PurchaseId);
                if (purchase != null)
                    purchase.TotalAmount = purchase.TotalAmount + delta;
            }

            // Single SaveChangesAsync persists the item mutation and any purchase-total change
            // because _itemRepository and _purchaseRepository share the same scoped DbContext.
            await _itemRepository.SaveChangesAsync();

            _logger.LogInformation("Item updated: {ItemId}", itemId);

            return _mapper.Map<PurchasedItemDetails>(item);
        }

        public async Task<bool> DeleteAsync(Guid itemId)
        {
            var item = await _itemRepository.GetPurchasedItemByIdAsync(itemId);
            if (item == null) return false;

            var purchaseId = item.PurchaseId;

            item.IsDeleted = true;
            item.IsActive = false;

            _itemRepository.Update(item);

            var purchase = await _purchaseRepository.GetPurchaseByIdAsync(purchaseId);
            if (purchase != null)
                purchase.TotalAmount = purchase.TotalAmount - item.SubTotal;

            // Single SaveChangesAsync persists the soft-delete and the purchase-total change.
            await _itemRepository.SaveChangesAsync();

            _logger.LogInformation("Item soft-deleted: {ItemId}", itemId);
            return true;
        }
    }
}

