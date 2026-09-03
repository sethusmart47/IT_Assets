using AutoMapper;
using ITAssetManagement.Dtos.Purchase;
using ITAssetManagement.Domain.Entities;
using ITAssetManagement.Domain.Enums;
using ITAssetManagement.Repositories.Interface;
using ITAssetManagement.Services.Interface;

namespace ITAssetManagement.Services.Implementations
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

        public async Task<List<PurchaseListItem>> GetAllPurchasesAsync(PurchaseStatus? status = null)
        {
            var entities = await _purchaseRepository.GetAllPurchasesAsync(status);
            return _mapper.Map<List<PurchaseListItem>>(entities);
        }

        public async Task<PurchaseDetails?> GetPurchaseDetailsAsync(Guid id)
        {
            var entity = await _purchaseRepository.GetPurchaseByIdWithDetailsAsync(id);
            if (entity == null) return null;
            return _mapper.Map<PurchaseDetails>(entity);
        }

        public async Task<NextPurchaseNumber> GetNextPurchaseNumberAsync()
        {
            var nextNumber = await _purchaseRepository.GetNextPurchaseNumberAsync();
            return new NextPurchaseNumber { PurchaseNumber = nextNumber };
        }

        public async Task<PurchaseDetails> CreatePurchaseAsync(PurchaseCreateRequest dto)
        {
            var invoiceExists = await _purchaseRepository.IsInvoiceNumberExistsAsync(dto.InvoiceNumber.Trim());
            if (invoiceExists)
                throw new InvalidOperationException($"Invoice number '{dto.InvoiceNumber}' already exists.");

            var purchaseNumber = await _purchaseRepository.GetNextPurchaseNumberAsync();

            var purchase = _mapper.Map<Purchase>(dto);
            purchase.Id = Guid.NewGuid();
            purchase.PurchaseNumber = purchaseNumber;
            purchase.InvoiceNumber = dto.InvoiceNumber.Trim();
            purchase.Remarks = dto.Remarks?.Trim();
            purchase.Status = PurchaseStatus.Inprogress;
            purchase.TotalAmount = 0;
            purchase.CreatedAt = DateTime.UtcNow;
            purchase.CreatedBy = "System";

            await _purchaseRepository.AddPurchaseAsync(purchase);
            await _purchaseRepository.SaveChangesAsync();

            _logger.LogInformation("Purchase created: {PurchaseNumber}, Status: InProgress", purchaseNumber);

            var created = await _purchaseRepository.GetPurchaseByIdWithDetailsAsync(purchase.Id);
            return _mapper.Map<PurchaseDetails>(created!);
        }

        public async Task<PurchaseDetails?> UpdatePurchaseAsync(Guid id, PurchaseUpdateRequest dto)
        {
            var purchase = await _purchaseRepository.GetPurchaseByIdAsync(id);
            if (purchase == null) return null;

            if (purchase.Status == PurchaseStatus.Received)
                throw new InvalidOperationException("Received purchase cannot be edited.");

            var daysSinceCreation = (DateTime.UtcNow - purchase.CreatedAt).TotalDays;
            if (daysSinceCreation > 5)
                throw new InvalidOperationException("Purchase can only be edited within 5 days after creation.");

            var invoiceExists = await _purchaseRepository.IsInvoiceNumberExistsAsync(dto.InvoiceNumber.Trim(), id);
            if (invoiceExists)
                throw new InvalidOperationException($"Invoice number '{dto.InvoiceNumber}' already exists.");

            purchase.VendorId = dto.VendorId;
            purchase.PurchaseDate = dto.PurchaseDate;
            purchase.InvoiceNumber = dto.InvoiceNumber.Trim();
            purchase.InvoiceDate = dto.InvoiceDate;
            purchase.ExpectedDeliveryDate = dto.ExpectedDeliveryDate;
            purchase.OwnershipType = dto.OwnershipType;
            purchase.Remarks = dto.Remarks?.Trim();
            purchase.ModifiedAt = DateTime.UtcNow;
            purchase.ModifiedBy = "System";

            _purchaseRepository.UpdatePurchase(purchase);
            await _purchaseRepository.SaveChangesAsync();

            _logger.LogInformation("Purchase updated: {Id}", id);

            var updated = await _purchaseRepository.GetPurchaseByIdWithDetailsAsync(id);
            return _mapper.Map<PurchaseDetails>(updated!);
        }

        public async Task<bool> ReceivePurchaseAsync(Guid id)
        {
            var purchase = await _purchaseRepository.GetPurchaseByIdWithDetailsAsync(id);
            if (purchase == null) return false;

            if (purchase.Status == PurchaseStatus.Received)
                throw new InvalidOperationException("Purchase is already received.");

            var activeItems = purchase.PurchasedItems.Where(i => !i.IsDeleted).ToList();
            if (activeItems.Count == 0)
                throw new InvalidOperationException("Cannot receive purchase without items. Add at least one item.");

            purchase.TotalAmount = activeItems.Sum(i => i.SubTotal);
            purchase.Status = PurchaseStatus.Received;
            purchase.ModifiedAt = DateTime.UtcNow;
            purchase.ModifiedBy = "System";

            _purchaseRepository.UpdatePurchase(purchase);
            await _purchaseRepository.SaveChangesAsync();

            _logger.LogInformation("Purchase received: {PurchaseNumber}", purchase.PurchaseNumber);
            return true;
        }

        public async Task<bool> DeletePurchaseAsync(Guid id)
        {
            var purchase = await _purchaseRepository.GetPurchaseByIdWithDetailsAsync(id);
            if (purchase == null) return false;

            if (purchase.Status == PurchaseStatus.Received)
                throw new InvalidOperationException("Received purchase cannot be deleted.");

            purchase.IsDeleted = true;
            purchase.IsActive = false;
            purchase.DeletedAt = DateTime.UtcNow;
            purchase.DeletedBy = "System";

            foreach (var item in purchase.PurchasedItems)
            {
                item.IsDeleted = true;
                item.IsActive = false;
                item.DeletedAt = DateTime.UtcNow;
                item.DeletedBy = "System";
            }

            foreach (var attachment in purchase.Attachments)
            {
                attachment.IsDeleted = true;
                attachment.IsActive = false;
                attachment.DeletedAt = DateTime.UtcNow;
                attachment.DeletedBy = "System";
            }

            _purchaseRepository.UpdatePurchase(purchase);
            await _purchaseRepository.SaveChangesAsync();

            _logger.LogInformation("Purchase soft-deleted: {PurchaseNumber}", purchase.PurchaseNumber);
            return true;
        }
    }
}
