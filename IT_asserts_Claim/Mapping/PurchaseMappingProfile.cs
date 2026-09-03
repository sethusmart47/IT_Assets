using AutoMapper;
using ITAssetManagement.Dtos.Purchase;
using ITAssetManagement.Dtos.PurchaseItem;
using ITAssetManagement.Domain.Entities;

namespace ITAssetManagement.Mapping
{
    
        public class PurchaseMappingProfile : Profile
        {
            public PurchaseMappingProfile()
            {
                // ═══════════════════════════════════════════════════════════════
                // PURCHASE MAPPINGS
                // ═══════════════════════════════════════════════════════════════

                CreateMap<PurchaseCreateRequest, Purchase>()
                    .ForMember(d => d.Id, opt => opt.Ignore())
                    .ForMember(d => d.PurchaseNumber, opt => opt.Ignore())
                    .ForMember(d => d.TotalAmount, opt => opt.Ignore())
                    .ForMember(d => d.Status, opt => opt.Ignore())
                    .ForMember(d => d.Vendor, opt => opt.Ignore())
                    .ForMember(d => d.PurchasedItems, opt => opt.Ignore())
                    .ForMember(d => d.Attachments, opt => opt.Ignore());

                CreateMap<PurchaseUpdateRequest, Purchase>()
                    .ForMember(d => d.Id, opt => opt.Ignore())
                    .ForMember(d => d.PurchaseNumber, opt => opt.Ignore())
                    .ForMember(d => d.TotalAmount, opt => opt.Ignore())
                    .ForMember(d => d.Status, opt => opt.Ignore())
                    .ForMember(d => d.Vendor, opt => opt.Ignore())
                    .ForMember(d => d.PurchasedItems, opt => opt.Ignore())
                    .ForMember(d => d.Attachments, opt => opt.Ignore());

                CreateMap<Purchase, PurchaseListItem>()
                    .ForMember(d => d.VendorName,
                        opt => opt.MapFrom(s => s.Vendor != null ? s.Vendor.VendorName : string.Empty))
                    .ForMember(d => d.OwnershipTypeName,
                        opt => opt.MapFrom(s => s.OwnershipType.ToString()))
                    .ForMember(d => d.StatusName,
                        opt => opt.MapFrom(s => s.Status.ToString()))
                    .ForMember(d => d.ItemCount,
                        opt => opt.MapFrom(s => s.PurchasedItems.Count(i => !i.IsDeleted)))
                    .ForMember(d => d.TotalUnits,
                        opt => opt.MapFrom(s => s.PurchasedItems.Where(i => !i.IsDeleted).Sum(i => i.Quantity)))
                    .ForMember(d => d.AttachmentCount,
                        opt => opt.MapFrom(s => s.Attachments.Count(a => !a.IsDeleted)));

                CreateMap<Purchase, PurchaseDetails>()
                    .ForMember(d => d.VendorName,
                        opt => opt.MapFrom(s => s.Vendor != null ? s.Vendor.VendorName : string.Empty))
                    .ForMember(d => d.OwnershipTypeName,
                        opt => opt.MapFrom(s => s.OwnershipType.ToString()))
                    .ForMember(d => d.StatusName,
                        opt => opt.MapFrom(s => s.Status.ToString()))
                    .ForMember(d => d.Items,
                        opt => opt.MapFrom(s => s.PurchasedItems.Where(i => !i.IsDeleted)))
                    .ForMember(d => d.Attachments,
                        opt => opt.MapFrom(s => s.Attachments.Where(a => !a.IsDeleted)));

                // ═══════════════════════════════════════════════════════════════
                // PURCHASED ITEM MAPPINGS (Simple — no FK, no Include needed)
                // ═══════════════════════════════════════════════════════════════

                CreateMap<PurchasedItemCreateRequest, PurchasedItem>()
                    .ForMember(d => d.Id, opt => opt.Ignore())
                    .ForMember(d => d.PurchaseId, opt => opt.Ignore())
                    .ForMember(d => d.SubTotal, opt => opt.Ignore())
                    .ForMember(d => d.Purchase, opt => opt.Ignore());

                CreateMap<PurchasedItemUpdateRequest, PurchasedItem>()
                    .ForMember(d => d.Id, opt => opt.Ignore())
                    .ForMember(d => d.PurchaseId, opt => opt.Ignore())
                    .ForMember(d => d.SubTotal, opt => opt.Ignore())
                    .ForMember(d => d.Purchase, opt => opt.Ignore());

                // Entity → DTO (direct map — all string fields match 1:1)
                CreateMap<PurchasedItem, PurchasedItemDetails>();



                CreateMap<PurchaseAttachment, PurchaseAttachmentDetails>()
                    .ForMember(d => d.FileSizeDisplay,
                        opt => opt.MapFrom(s => FormatFileSize(s.FileSize)));
            }

            private static string FormatFileSize(long bytes)
            {
                if (bytes >= 1_048_576) return $"{bytes / 1_048_576.0:F1} MB";
                if (bytes >= 1_024) return $"{bytes / 1_024.0:F1} KB";
                return $"{bytes} B";
            }
        }
    }
