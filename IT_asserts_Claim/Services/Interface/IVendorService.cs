using ITAssetManagement.Dtos.Vendor;

namespace ITAssetManagement.Services.Interface
{
    public interface IVendorService
    {
        Task<List<VendorDetails>> GetAllAsync();
        Task<VendorDetails?> GetByIdAsync(Guid id);
        Task<List<VendorDropdownItem>> GetDropdownAsync();
        Task<VendorDetails> CreateAsync(VendorCreateRequest dto);
        Task<VendorDetails?> UpdateAsync(Guid id, VendorUpdateRequest dto);
        Task<bool> DeleteAsync(Guid id);
    }
}
