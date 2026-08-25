using AutoMapper;
using IT_asserts.Dtos.Asset;
using IT_asserts.Repositories.Interface;
using IT_asserts.Services.Interface;
using IT_asserts_Claim.Data;
using IT_asserts_Claim.Dtos.Asset;
using IT_asserts_Claim.entity;
using IT_asserts_Claim.Enum;
using Microsoft.EntityFrameworkCore;

namespace IT_asserts.Services.Implementations
{
    public class AssetService : IAssetService
    {
        private readonly IAssetRepository _assetRepository;
        private readonly IAssetLifecycleHistoryRepository _historyRepository;
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public AssetService(
            IAssetRepository assetRepository,
            IAssetLifecycleHistoryRepository historyRepository,
            AppDbContext context,
            IMapper mapper)
        {
            _assetRepository = assetRepository;
            _historyRepository = historyRepository;
            _context = context;
            _mapper = mapper;
        }

        // ─── GET ALL ASSETS ──────────────────────────────────────────────────
        public async Task<List<AssetListDto>> GetAllAsync(string? status, string? category, string? brand, string? search)
        {
            var assets = await _assetRepository.GetAllAsync(status, category, brand, search);
            return _mapper.Map<List<AssetListDto>>(assets);
        }

        // ─── GET BY ID (Detail) ──────────────────────────────────────────────
        public async Task<AssetDetailDto?> GetByIdAsync(Guid id)
        {
            var asset = await _assetRepository.GetByIdWithDetailsAsync(id);
            if (asset == null) return null;
            return _mapper.Map<AssetDetailDto>(asset);
        }

        // ─── REGISTER SINGLE ASSET ───────────────────────────────────────────
        public async Task<AssetDto> RegisterAsync(CreateAssetDto dto)
        {
            // Validate serial number
            var exists = await _assetRepository.SerialNumberExistsAsync(dto.SerialNumber.Trim());
            if (exists)
                throw new InvalidOperationException($"Serial number '{dto.SerialNumber}' already exists.");

            // Generate asset tag
            var assetTag = await GenerateAssetTagAsync(dto.Category);

            // Manual mapping: DTO → Entity
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

            await _assetRepository.AddAsync(asset);

            // Create lifecycle history
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

            await _historyRepository.AddAsync(history);
            await _assetRepository.SaveChangesAsync();

            // Reload with navigation properties for response
            var savedAsset = await _assetRepository.GetByIdAsync(asset.Id);
            return _mapper.Map<AssetDto>(savedAsset!);
        }

        // ─── BULK REGISTER ASSETS ────────────────────────────────────────────
        public async Task<List<AssetDto>> BulkRegisterAsync(BulkCreateAssetDto dto)
        {
            if (dto.SerialNumbers == null || dto.SerialNumbers.Count == 0)
                throw new InvalidOperationException("At least one serial number is required.");

            var trimmedSerials = dto.SerialNumbers.Select(s => s.Trim()).ToList();

            // Check duplicates within the list
            var duplicatesInList = trimmedSerials
                .GroupBy(s => s, StringComparer.OrdinalIgnoreCase)
                .Where(g => g.Count() > 1)
                .Select(g => g.Key)
                .ToList();

            if (duplicatesInList.Any())
                throw new InvalidOperationException($"Duplicate serial numbers in request: {string.Join(", ", duplicatesInList)}");

            // Check existing serial numbers in database
            var existingSerials = await _assetRepository.GetExistingSerialNumbersAsync(trimmedSerials);
            if (existingSerials.Any())
                throw new InvalidOperationException($"Serial numbers already exist: {string.Join(", ", existingSerials)}");

            var assets = new List<Asset>();
            var histories = new List<AssetLifecycleHistory>();

            foreach (var serial in trimmedSerials)
            {
                var assetTag = await GenerateAssetTagAsync(dto.Category);

                var asset = new Asset
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
                };

                assets.Add(asset);

                histories.Add(new AssetLifecycleHistory
                {
                    AssetId = asset.Id,
                    Action = "Registered",
                    OldStatus = null,
                    NewStatus = AssetStatus.Available,
                    Remarks = "Asset registered from purchase (bulk)",
                    PerformedBy = "System",
                    PerformedDate = DateTime.UtcNow
                });
            }

            await _assetRepository.AddRangeAsync(assets);
            await _historyRepository.AddRangeAsync(histories);
            await _assetRepository.SaveChangesAsync();

            // Reload with navigation properties
            var assetIds = assets.Select(a => a.Id).ToList();
            var savedAssets = await _context.Assets
                .AsNoTracking()
                .Include(a => a.Purchase)
                    .ThenInclude(p => p.Vendor)
                .Where(a => assetIds.Contains(a.Id))
                .OrderBy(a => a.AssetTag)
                .ToListAsync();

            return _mapper.Map<List<AssetDto>>(savedAssets);
        }

        // ─── UPDATE ASSET ────────────────────────────────────────────────────
        public async Task<AssetDto?> UpdateAsync(Guid id, UpdateAssetDto dto)
        {
            var asset = await _assetRepository.GetByIdAsync(id);
            if (asset == null) return null;

            var oldCondition = asset.Condition;

            // Manual update
            asset.Condition = (AssetCondition)dto.Condition;
            asset.Remarks = dto.Remarks?.Trim();

            // Create lifecycle history if condition changed
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

                await _historyRepository.AddAsync(history);
            }

            await _assetRepository.SaveChangesAsync();
            return _mapper.Map<AssetDto>(asset);
        }

        // ─── SOFT DELETE ──────────────────────────────────────────────────────
        public async Task<bool> DeleteAsync(Guid id)
        {
            var asset = await _assetRepository.GetByIdAsync(id);
            if (asset == null) return false;

            asset.IsDeleted = true;
            await _assetRepository.SaveChangesAsync();
            return true;
        }

        // ─── VALIDATE SERIAL NUMBERS ─────────────────────────────────────────
        public async Task<ValidationResultDto> ValidateSerialNumbersAsync(ValidateSerialNumbersDto dto)
        {
            var result = new ValidationResultDto();
            var trimmedSerials = dto.SerialNumbers.Select(s => s.Trim()).ToList();

            var existingSerials = await _assetRepository.GetExistingSerialNumbersAsync(trimmedSerials);
            var seenSerials = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            for (int i = 0; i < trimmedSerials.Count; i++)
            {
                var serial = trimmedSerials[i];

                if (string.IsNullOrWhiteSpace(serial))
                {
                    result.Errors.Add(new ValidationError
                    {
                        Index = i + 1,
                        SerialNumber = serial,
                        Error = "Serial number is required"
                    });
                }
                else if (!seenSerials.Add(serial))
                {
                    result.Errors.Add(new ValidationError
                    {
                        Index = i + 1,
                        SerialNumber = serial,
                        Error = "Duplicate serial number in this list"
                    });
                }
                else if (existingSerials.Contains(serial))
                {
                    result.Errors.Add(new ValidationError
                    {
                        Index = i + 1,
                        SerialNumber = serial,
                        Error = "Serial number already exists in the system"
                    });
                }
                else
                {
                    result.ValidSerials.Add(serial);
                }
            }

            return result;
        }

        // ─── GET AVAILABLE PURCHASES ─────────────────────────────────────────
        public async Task<List<AvailablePurchaseDto>> GetAvailablePurchasesAsync()
        {
            var purchases = await _context.Purchases
                .AsNoTracking()
                .Include(p => p.Vendor)
                .Include(p => p.PurchasedItems.Where(i => !i.IsDeleted))
                .Where(p => !p.IsDeleted)
                .Where(p=>p.Status==PurchaseStatus.Completed)
                .ToListAsync();

            var result = new List<AvailablePurchaseDto>();

            foreach (var purchase in purchases)
            {
                var totalQty = purchase.PurchasedItems.Sum(i => i.Quantity);
                var registeredQty = await _assetRepository.GetRegisteredCountByPurchaseAsync(purchase.Id);
                var remainingQty = totalQty - registeredQty;

                if (remainingQty > 0)
                {
                    result.Add(new AvailablePurchaseDto
                    {
                        Id = purchase.Id,
                        PurchaseNumber = purchase.PurchaseNumber,
                        VendorName = purchase.Vendor.VendorName,
                        PurchaseDate = purchase.PurchaseDate,
                        TotalItems = purchase.PurchasedItems.Count,
                        TotalQty = totalQty,
                        RegisteredQty = registeredQty,
                        RemainingQty = remainingQty
                    });
                }
            }

            return result.OrderByDescending(p => p.PurchaseDate).ToList();
        }

        // ─── GET AVAILABLE ITEMS FOR A PURCHASE ──────────────────────────────
        public async Task<List<AvailablePurchaseItemDto>> GetAvailableItemsAsync(Guid purchaseId)
        {
            var purchaseItems = await _context.PurchasedItems
                .AsNoTracking()
                .Where(i => i.PurchaseId == purchaseId && !i.IsDeleted)
                .ToListAsync();

            var result = new List<AvailablePurchaseItemDto>();

            foreach (var item in purchaseItems)
            {
                var registeredQty = await _assetRepository.GetRegisteredCountByPurchaseItemAsync(
                    purchaseId, item.Category, item.Brand, item.Model);

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
                        WarrantyMonths = item.WarrantyPeriod,
                        PurchasedQty = item.Quantity,
                        RegisteredQty = registeredQty,
                        RemainingQty = remainingQty
                    });
                }
            }

            return result;
        }

        // ─── GET REGISTRATION SUMMARY ────────────────────────────────────────
        public async Task<RegistrationSummaryDto?> GetRegistrationSummaryAsync(Guid purchaseId)
        {
            var purchase = await _context.Purchases
                .AsNoTracking()
                .Include(p => p.PurchasedItems.Where(i => !i.IsDeleted))
                .FirstOrDefaultAsync(p => p.Id == purchaseId && !p.IsDeleted);

            if (purchase == null) return null;

            var totalPurchasedQty = purchase.PurchasedItems.Sum(i => i.Quantity);
            var registeredQty = await _assetRepository.GetRegisteredCountByPurchaseAsync(purchaseId);

            return new RegistrationSummaryDto
            {
                PurchaseId = purchase.Id,
                PurchaseNumber = purchase.PurchaseNumber,
                TotalPurchasedQty = totalPurchasedQty,
                RegisteredQty = registeredQty,
                RemainingQty = totalPurchasedQty - registeredQty
            };
        }

        // ─── PRIVATE: Generate Asset Tag ─────────────────────────────────────
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
