using AutoMapper;
using IT_asserts_Claim.Dtos.PurchaseItem;
using IT_asserts_Claim.entity;
using IT_asserts_Claim.Repositories.Interface;
using IT_asserts_Claim.Services.Interface;

namespace IT_asserts_Claim.Services.Implementations
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

        public async Task<List<PurchasedItemDto>> GetAllByPurchaseIdAsync(Guid purchaseId)
        {
            var items = await _itemRepository.GetAllByPurchaseIdAsync(purchaseId);
            return _mapper.Map<List<PurchasedItemDto>>(items);
        }

        public async Task<PurchasedItemDto?> GetByIdAsync(Guid itemId)
        {
            var item = await _itemRepository.GetByIdAsync(itemId);
            if (item == null) return null;
            return _mapper.Map<PurchasedItemDto>(item);
        }

        public async Task<PurchasedItemDto> CreateAsync(Guid purchaseId, CreatePurchasedItemDto dto)
        {
            // Validate purchase exists
            var purchase = await _purchaseRepository.GetByIdAsync(purchaseId);
            if (purchase == null)
                throw new InvalidOperationException("Purchase not found.");

            // Map DTO → Entity
            var item = _mapper.Map<PurchasedItem>(dto);
            item.Id = Guid.NewGuid();
            item.PurchaseId = purchaseId;
            item.SubTotal = dto.Quantity * dto.UnitPrice;

            await _itemRepository.AddAsync(item);
            await _itemRepository.SaveChangesAsync();

            // Recalculate purchase total
            await RecalculatePurchaseTotalAsync(purchaseId);

            _logger.LogInformation("Item added: {Category} | {Brand} | {Model} → Purchase {PurchaseId}",
                dto.Category, dto.Brand, dto.Model, purchaseId);

            return _mapper.Map<PurchasedItemDto>(item);
        }

        public async Task<PurchasedItemDto?> UpdateAsync(Guid itemId, UpdatePurchasedItemDto dto)
        {
            var item = await _itemRepository.GetByIdAsync(itemId);
            if (item == null) return null;

            // Update fields directly
            item.Category = dto.Category.Trim();
            item.Brand = dto.Brand.Trim();
            item.Model = dto.Model.Trim();
            item.Configuration = dto.Configuration.Trim();
            item.Quantity = dto.Quantity;
            item.UnitPrice = dto.UnitPrice;
            item.WarrantyPeriod = dto.WarrantyPeriod;
            item.SubTotal = dto.Quantity * dto.UnitPrice;

            _itemRepository.Update(item);
            await _itemRepository.SaveChangesAsync();

            // Recalculate purchase total
            await RecalculatePurchaseTotalAsync(item.PurchaseId);

            _logger.LogInformation("Item updated: {ItemId}", itemId);

            return _mapper.Map<PurchasedItemDto>(item);
        }

        public async Task<bool> DeleteAsync(Guid itemId)
        {
            var item = await _itemRepository.GetByIdAsync(itemId);
            if (item == null) return false;

            var purchaseId = item.PurchaseId;

            item.IsDeleted = true;
            item.IsActive = false;

            _itemRepository.Update(item);
            await _itemRepository.SaveChangesAsync();

            // Recalculate purchase total
            await RecalculatePurchaseTotalAsync(purchaseId);

            _logger.LogInformation("Item soft-deleted: {ItemId}", itemId);
            return true;
        }

        // ─── PRIVATE ───

        private async Task RecalculatePurchaseTotalAsync(Guid purchaseId)
        {
            var purchase = await _purchaseRepository.GetByIdAsync(purchaseId);
            if (purchase == null) return;

            var items = await _itemRepository.GetAllByPurchaseIdAsync(purchaseId);
            purchase.TotalAmount = items.Sum(i => i.SubTotal);

            _purchaseRepository.Update(purchase);
            await _purchaseRepository.SaveChangesAsync();
        }
    }
}
