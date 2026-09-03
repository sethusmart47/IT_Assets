using AutoMapper;
using ITAssetManagement.Domain.Entities;
using ITAssetManagement.Domain.Enums;
using ITAssetManagement.Dtos.Asset;
using ITAssetManagement.Repositories.Interface;
using ITAssetManagement.Services.Interface;

namespace ITAssetManagement.Services.Implementations
{
    /// <summary>
    /// Business logic for assets. All data access goes through repositories — no DbContext here.
    /// </summary>
    public class AssetService : IAssetService
    {
        private readonly IAssetRepository _assetRepository;
        private readonly IAssetLifecycleHistoryRepository _historyRepository;
        private readonly IPurchaseRepository _purchaseRepository;
        private readonly IPurchasedItemRepository _purchasedItemRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AssetService(
            IAssetRepository assetRepository,
            IAssetLifecycleHistoryRepository historyRepository,
            IPurchaseRepository purchaseRepository,
            IPurchasedItemRepository purchasedItemRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _assetRepository = assetRepository;
            _historyRepository = historyRepository;
            _purchaseRepository = purchaseRepository;
            _purchasedItemRepository = purchasedItemRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<AssetListItem>> GetAllAsync(string? status, string? category, string? brand, string? search)
        {
            var assets = await _assetRepository.GetAllAssetsAsync(status, category, brand, search);
            return _mapper.Map<List<AssetListItem>>(assets);
        }

        public async Task<AssetDetailsFull?> GetByIdAsync(Guid id)
        {
            var asset = await _assetRepository.GetAssetByIdWithDetailsAsync(id);
            return asset == null ? null : _mapper.Map<AssetDetailsFull>(asset);
        }

        public async Task<AssetDetails> RegisterAsync(AssetCreateRequest dto)
        {
            if (await _assetRepository.SerialNumberExistsAsync(dto.SerialNumber.Trim()))
                throw new InvalidOperationException($"Serial number '{dto.SerialNumber}' already exists.");

            var assetTag = await GenerateAssetTagAsync(dto.Category);

            var asset = new Asset
            {
                AssetTag = assetTag,
                SerialNumber = dto.SerialNumber.Trim(),
                PurchaseId = dto.PurchaseId,
                Category = dto.Category.Trim(),
                Brand = dto.Brand.Trim(),
                Model = dto.Model.Trim(),
                Configuration = dto.Configuration?.Trim(),
                Status = AssetStatus.Available,
                Condition = (AssetCondition)dto.Condition,
                OwnershipType = (OwnershipType)dto.OwnershipType,
                WarrantyStartDate = dto.WarrantyStartDate,
                WarrantyEndDate = dto.WarrantyEndDate,
                WarrantyMonths = dto.WarrantyMonths,
                Remarks = dto.Remarks?.Trim()
            };

            await _assetRepository.AddAssetAsync(asset);

            var history = new AssetLifecycleHistory
            {
                AssetId = asset.Id,
                Action = "Registered",
                OldStatus = null,
                NewStatus = AssetStatus.Available,
                Remarks = "Asset registered from purchase",
                PerformedBy = "System",
                PerformedDate = DateTime.UtcNow
            };

            await _historyRepository.AddAssetLifecycleHistoryAsync(history);
            await _unitOfWork.SaveChangesAsync();

            var savedAsset = await _assetRepository.GetAssetByIdAsync(asset.Id);
            return _mapper.Map<AssetDetails>(savedAsset!);
        }

        public async Task<List<AssetDetails>> BulkRegisterAsync(AssetBulkCreateRequest dto)
        {
            if (dto.SerialNumbers == null || dto.SerialNumbers.Count == 0)
                throw new InvalidOperationException("At least one serial number is required.");

            var trimmedSerials = dto.SerialNumbers.Select(s => s.Trim()).ToList();

            var duplicatesInList = trimmedSerials
                .GroupBy(s => s, StringComparer.OrdinalIgnoreCase)
                .Where(g => g.Count() > 1)
                .Select(g => g.Key)
                .ToList();
            if (duplicatesInList.Any())
                throw new InvalidOperationException($"Duplicate serial numbers in request: {string.Join(", ", duplicatesInList)}");

            var existingSerials = await _assetRepository.GetExistingSerialNumbersAsync(trimmedSerials);
            if (existingSerials.Any())
                throw new InvalidOperationException($"Serial numbers already exist: {string.Join(", ", existingSerials)}");

            var prefix = GetCategoryPrefix(dto.Category);
            var year = DateTime.UtcNow.Year;
            var pattern = $"AST-{prefix}-{year}-";
            var lastTag = await _assetRepository.GetLastAssetTagAsync(pattern);
            var nextSequence = 1;
            if (lastTag != null)
            {
                var lastSequenceStr = lastTag.Substring(pattern.Length);
                if (int.TryParse(lastSequenceStr, out var lastSequence))
                    nextSequence = lastSequence + 1;
            }

            var assets = new List<Asset>();
            foreach (var serial in trimmedSerials)
            {
                var assetTag = $"{pattern}{nextSequence:D4}";
                nextSequence++;
                assets.Add(new Asset
                {
                    AssetTag = assetTag,
                    SerialNumber = serial,
                    PurchaseId = dto.PurchaseId,
                    Category = dto.Category.Trim(),
                    Brand = dto.Brand.Trim(),
                    Model = dto.Model.Trim(),
                    Configuration = dto.Configuration?.Trim(),
                    Status = AssetStatus.Available,
                    Condition = (AssetCondition)dto.Condition,
                    OwnershipType = (OwnershipType)dto.OwnershipType,
                    WarrantyStartDate = dto.WarrantyStartDate,
                    WarrantyEndDate = dto.WarrantyEndDate,
                    WarrantyMonths = dto.WarrantyMonths,
                    Remarks = dto.Remarks?.Trim()
                });
            }

            await _assetRepository.AddAssetsAsync(assets);

            var histories = assets.Select(asset => new AssetLifecycleHistory
            {
                AssetId = asset.Id,
                Action = "Registered",
                OldStatus = null,
                NewStatus = AssetStatus.Available,
                Remarks = "Asset registered from purchase (bulk)",
                PerformedBy = "System",
                PerformedDate = DateTime.UtcNow
            }).ToList();

            await _historyRepository.AddAssetLifecycleHistoriesAsync(histories);

            // Single transactional save for both assets and histories
            await _unitOfWork.SaveChangesAsync();

            var assetIds = assets.Select(a => a.Id).ToList();
            var savedAssets = await _assetRepository.GetByIdsWithDetailsAsync(assetIds);
            return _mapper.Map<List<AssetDetails>>(savedAssets);
        }

        public async Task<AssetDetails?> UpdateAsync(Guid id, AssetUpdateRequest dto)
        {
            var asset = await _assetRepository.GetAssetByIdAsync(id);
            if (asset == null) return null;

            var oldCondition = asset.Condition;
            asset.Condition = (AssetCondition)dto.Condition;
            asset.Remarks = dto.Remarks?.Trim();

            if (oldCondition != (AssetCondition)dto.Condition)
            {
                var history = new AssetLifecycleHistory
                {
                    AssetId = asset.Id,
                    Action = "ConditionChanged",
                    OldStatus = asset.Status,
                    NewStatus = asset.Status,
                    Remarks = $"Condition changed from {oldCondition} to {(AssetCondition)dto.Condition}",
                    PerformedBy = "System",
                    PerformedDate = DateTime.UtcNow
                };
                await _historyRepository.AddAssetLifecycleHistoryAsync(history);
            }

            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<AssetDetails>(asset);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var asset = await _assetRepository.GetAssetByIdAsync(id);
            if (asset == null) return false;
            asset.IsDeleted = true;
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<AssetValidationResult> ValidateSerialNumbersAsync(AssetValidateSerialNumbersRequest dto)
        {
            var result = new AssetValidationResult();
            var trimmedSerials = dto.SerialNumbers.Select(s => s.Trim()).ToList();
            var existingSerials = await _assetRepository.GetExistingSerialNumbersAsync(trimmedSerials);
            var seenSerials = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            for (int i = 0; i < trimmedSerials.Count; i++)
            {
                var serial = trimmedSerials[i];
                if (string.IsNullOrWhiteSpace(serial))
                {
                    result.Errors.Add(new AssetValidationError { Index = i + 1, SerialNumber = serial, Error = "Serial number is required" });
                }
                else if (!seenSerials.Add(serial))
                {
                    result.Errors.Add(new AssetValidationError { Index = i + 1, SerialNumber = serial, Error = "Duplicate serial number in this list" });
                }
                else if (existingSerials.Contains(serial, StringComparer.OrdinalIgnoreCase))
                {
                    result.Errors.Add(new AssetValidationError { Index = i + 1, SerialNumber = serial, Error = "Serial number already exists in the system" });
                }
                else
                {
                    result.ValidSerials.Add(serial);
                }
            }
            return result;
        }

        public async Task<List<AvailablePurchaseListItem>> GetAvailablePurchasesAsync()
        {
            var purchases = await _purchaseRepository.GetConfirmedWithItemsAsync();

            // Batch query: single DB round-trip for all registered counts (avoids N+1).
            var registeredCounts = await _assetRepository
                .GetRegisteredCountsByPurchasesAsync(purchases.Select(p => p.Id));

            var result = new List<AvailablePurchaseListItem>();

            foreach (var purchase in purchases)
            {
                var totalQty = purchase.PurchasedItems.Sum(i => i.Quantity);
                var registeredQty = registeredCounts.GetValueOrDefault(purchase.Id);
                var remainingQty = totalQty - registeredQty;
                if (remainingQty > 0)
                {
                    result.Add(new AvailablePurchaseListItem
                    {
                        Id = purchase.Id,
                        PurchaseNumber = purchase.PurchaseNumber,
                        VendorName = purchase.Vendor.VendorName,
                        PurchaseDate = purchase.PurchaseDate,
                        TotalItems = purchase.PurchasedItems.Count,
                        OwnershipType = purchase.OwnershipType,
                        OwnershipTypeName = purchase.OwnershipType.ToString(),
                        TotalQty = totalQty,
                        RegisteredQty = registeredQty,
                        RemainingQty = remainingQty
                    });
                }
            }
            return result.OrderByDescending(p => p.PurchaseDate).ToList();
        }

        public async Task<List<AvailablePurchaseItemDto>> GetAvailableItemsAsync(Guid purchaseId)
        {
            var purchaseItems = await _purchasedItemRepository.GetAllByPurchaseIdAsync(purchaseId);

            // Batch query: single DB round-trip for all registered counts (avoids N+1).
            var registeredCounts = await _assetRepository.GetRegisteredCountsByPurchaseItemsAsync(purchaseId);

            var result = new List<AvailablePurchaseItemDto>();

            foreach (var item in purchaseItems)
            {
                var registeredQty = registeredCounts.GetValueOrDefault((item.Category, item.Brand, item.Model));
                var remainingQty = item.Quantity - registeredQty;
                if (remainingQty > 0)
                {
                    result.Add(new AvailablePurchaseItemDto
                    {
                        PurchaseItemId = item.Id,
                        Category = item.Category,
                        Brand = item.Brand,
                        Model = item.Model,
                        Configuration = item.Configuration,
                        UnitPrice = item.UnitPrice,
                        PurchasedQty = item.Quantity,
                        RegisteredQty = registeredQty,
                        RemainingQty = remainingQty
                    });
                }
            }
            return result;
        }

        public async Task<AssetRegistrationSummary?> GetRegistrationSummaryAsync(Guid purchaseId)
        {
            var purchase = await _purchaseRepository.GetByIdWithItemsAsync(purchaseId);
            if (purchase == null) return null;

            var totalPurchasedQty = purchase.PurchasedItems.Sum(i => i.Quantity);
            var registeredQty = await _assetRepository.GetRegisteredCountByPurchaseAsync(purchaseId);

            return new AssetRegistrationSummary
            {
                PurchaseId = purchase.Id,
                PurchaseNumber = purchase.PurchaseNumber,
                TotalPurchasedQty = totalPurchasedQty,
                RegisteredQty = registeredQty,
                RemainingQty = totalPurchasedQty - registeredQty
            };
        }

        public async Task<AssetDetailsFull?> SearchAvailableAssetAsync(string? serialNumber, string? assetTag)
        {
            Asset? asset = null;
            if (!string.IsNullOrWhiteSpace(serialNumber))
                asset = await _assetRepository.GetAvailableBySerialNumberAsync(serialNumber.Trim());
            else if (!string.IsNullOrWhiteSpace(assetTag))
                asset = await _assetRepository.GetAvailableByAssetTagAsync(assetTag.Trim());

            return asset == null ? null : _mapper.Map<AssetDetailsFull>(asset);
        }

        private async Task<string> GenerateAssetTagAsync(string category)
        {
            var prefix = GetCategoryPrefix(category);
            var year = DateTime.UtcNow.Year;
            var pattern = $"AST-{prefix}-{year}-";
            var lastTag = await _assetRepository.GetLastAssetTagAsync(pattern);
            var nextSequence = 1;
            if (lastTag != null)
            {
                var lastSequenceStr = lastTag.Substring(pattern.Length);
                if (int.TryParse(lastSequenceStr, out var lastSequence))
                    nextSequence = lastSequence + 1;
            }
            return $"{pattern}{nextSequence:D4}";
        }

        private static string GetCategoryPrefix(string category)
        {
            return category.ToUpper() switch
            {
                "LAPTOP" => "LAP",
                "MONITOR" => "MON",
                "KEYBOARD" => "KEY",
                "MOUSE" => "MOU",
                "HEADSET" => "HDS",
                "PRINTER" => "PRT",
                "DESKTOP" => "DSK",
                "TABLET" => "TAB",
                "PHONE" => "PHN",
                "SERVER" => "SRV",
                "NETWORKING" => "NET",
                _ => category.Length >= 3 ? category[..3].ToUpper() : category.ToUpper()
            };
        }
    }
}
