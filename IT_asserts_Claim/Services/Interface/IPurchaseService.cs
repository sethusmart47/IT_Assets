using ITAssetManagement.Dtos.Purchase;
using ITAssetManagement.Domain.Enums;

namespace ITAssetManagement.Services.Interface
{
    public interface IPurchaseService
    {
        Task<List<PurchaseListItem>> GetAllPurchasesAsync(PurchaseStatus? status = null);
        Task<PurchaseDetails?> GetPurchaseDetailsAsync(Guid id);
        Task<NextPurchaseNumber> GetNextPurchaseNumberAsync();
        Task<PurchaseDetails> CreatePurchaseAsync(PurchaseCreateRequest dto);
        Task<PurchaseDetails?> UpdatePurchaseAsync(Guid id, PurchaseUpdateRequest dto);
        Task<bool> ConfirmPurchaseAsync(Guid id);
        Task<bool> CancelPurchaseAsync(Guid id);
        Task<bool> DeletePurchaseAsync(Guid id);
    }
}