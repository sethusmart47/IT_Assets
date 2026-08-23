using AutoMapper;
using IT_asserts_Claim.Dtos.Purchase;
using IT_asserts_Claim.entity;
using IT_asserts_Claim.Enum;
using IT_asserts_Claim.Repositories.Interface;
using IT_asserts_Claim.Services.Interface;

namespace IT_asserts_Claim.Services.Implementations
{
    public class PurchaseService : IPurchaseService
    {
        private readonly IPurchaseRepository _purchaseRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<PurchaseService> _logger;

        public PurchaseService(
            IPurchaseRepository purchaseRepository,
            IMapper mapper,
            ILogger<PurchaseService> logger)
        {
            _purchaseRepository = purchaseRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<List<PurchaseListDto>> GetAllAsync(PurchaseStatus? status = null)
        {
            var entities = await _purchaseRepository.GetAllAsync(status);
            return _mapper.Map<List<PurchaseListDto>>(entities);
        }

        public async Task<PurchaseDetailDto?> GetByIdAsync(Guid id)
        {
            var entity = await _purchaseRepository.GetByIdWithDetailsAsync(id);
            if (entity == null) return null;
            return _mapper.Map<PurchaseDetailDto>(entity);
        }

        public async Task<NextPurchaseNumberDto> GetNextPurchaseNumberAsync()
        {
            var nextNumber = await _purchaseRepository.GetNextPurchaseNumberAsync();
            return new NextPurchaseNumberDto { PurchaseNumber = nextNumber };
        }

        public async Task<PurchaseDetailDto> CreateAsync(CreatePurchaseDto dto)
        {
            // Validate unique invoice
            var invoiceExists = await _purchaseRepository.IsInvoiceNumberExistsAsync(dto.InvoiceNumber.Trim());
            if (invoiceExists)
                throw new InvalidOperationException($"Invoice number '{dto.InvoiceNumber}' already exists.");

            // Generate purchase number
            var purchaseNumber = await _purchaseRepository.GetNextPurchaseNumberAsync();

            // Map & set
            var purchase = _mapper.Map<Purchase>(dto);
            purchase.Id = Guid.NewGuid();
            purchase.PurchaseNumber = purchaseNumber;
            purchase.InvoiceNumber = dto.InvoiceNumber.Trim();
            purchase.Remarks = dto.Remarks?.Trim();
            purchase.Status = PurchaseStatus.Draft;
            purchase.TotalAmount = 0;

            await _purchaseRepository.AddAsync(purchase);
            await _purchaseRepository.SaveChangesAsync();

            _logger.LogInformation("Purchase created: {PurchaseNumber}, Status: Draft", purchaseNumber);

            // Reload with vendor
            var created = await _purchaseRepository.GetByIdWithDetailsAsync(purchase.Id);
            return _mapper.Map<PurchaseDetailDto>(created!);
        }

        public async Task<PurchaseDetailDto?> UpdateAsync(Guid id, UpdatePurchaseDto dto)
        {
            var purchase = await _purchaseRepository.GetByIdAsync(id);
            if (purchase == null) return null;

            // Validate unique invoice (exclude self)
            var invoiceExists = await _purchaseRepository.IsInvoiceNumberExistsAsync(dto.InvoiceNumber.Trim(), id);
            if (invoiceExists)
                throw new InvalidOperationException($"Invoice number '{dto.InvoiceNumber}' already exists.");

            // Update
            purchase.VendorId = dto.VendorId;
            purchase.PurchaseDate = dto.PurchaseDate;
            purchase.InvoiceNumber = dto.InvoiceNumber.Trim();
            purchase.InvoiceDate = dto.InvoiceDate;
            purchase.ExpectedDeliveryDate = dto.ExpectedDeliveryDate;
            purchase.OwnershipType = dto.OwnershipType;
            purchase.Remarks = dto.Remarks?.Trim();

            _purchaseRepository.Update(purchase);
            await _purchaseRepository.SaveChangesAsync();

            _logger.LogInformation("Purchase updated: {Id}", id);

            var updated = await _purchaseRepository.GetByIdWithDetailsAsync(id);
            return _mapper.Map<PurchaseDetailDto>(updated!);
        }

        public async Task<bool> CompletePurchaseAsync(Guid id)
        {
            var purchase = await _purchaseRepository.GetByIdWithDetailsAsync(id);
            if (purchase == null) return false;

            var activeItems = purchase.PurchasedItems.Where(i => !i.IsDeleted).ToList();
            if (activeItems.Count == 0)
                throw new InvalidOperationException("Cannot complete purchase without items. Add at least one item.");

            purchase.TotalAmount = activeItems.Sum(i => i.SubTotal);
            purchase.Status = PurchaseStatus.Completed;

            _purchaseRepository.Update(purchase);
            await _purchaseRepository.SaveChangesAsync();

            _logger.LogInformation("Purchase completed: {PurchaseNumber}", purchase.PurchaseNumber);
            return true;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var purchase = await _purchaseRepository.GetByIdWithDetailsAsync(id);
            if (purchase == null) return false;

            purchase.IsDeleted = true;
            purchase.IsActive = false;

            foreach (var item in purchase.PurchasedItems)
            {
                item.IsDeleted = true;
                item.IsActive = false;
            }

            foreach (var attachment in purchase.Attachments)
            {
                attachment.IsDeleted = true;
                attachment.IsActive = false;
            }

            _purchaseRepository.Update(purchase);
            await _purchaseRepository.SaveChangesAsync();

            _logger.LogInformation("Purchase soft-deleted: {PurchaseNumber}", purchase.PurchaseNumber);
            return true;
        }
    }
}