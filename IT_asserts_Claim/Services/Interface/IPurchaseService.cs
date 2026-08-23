using IT_asserts_Claim.Dtos.Purchase;
using IT_asserts_Claim.Enum;

namespace IT_asserts_Claim.Services.Interface
{
    public interface IPurchaseService
    {
        Task<List<PurchaseListDto>> GetAllAsync(PurchaseStatus? status = null);
        Task<PurchaseDetailDto?> GetByIdAsync(Guid id);
        Task<NextPurchaseNumberDto> GetNextPurchaseNumberAsync();
        Task<PurchaseDetailDto> CreateAsync(CreatePurchaseDto dto);
        Task<PurchaseDetailDto?> UpdateAsync(Guid id, UpdatePurchaseDto dto);
        Task<bool> CompletePurchaseAsync(Guid id);
        Task<bool> DeleteAsync(Guid id);
    }
}
