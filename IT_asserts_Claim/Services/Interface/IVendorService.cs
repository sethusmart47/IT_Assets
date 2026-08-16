using IT_asserts_Claim.Dtos.Vendor;

namespace IT_asserts_Claim.Services.Interface
{
    public interface IVendorService
    {
        Task<List<VendorDto>> GetAllAsync();
        Task<VendorDto?> GetByIdAsync(Guid id);
        Task<List<VendorDropdownDto>> GetDropdownAsync();
        Task<VendorDto> CreateAsync(CreateVendorDto dto);
        Task<VendorDto?> UpdateAsync(Guid id, UpdateVendorDto dto);
        Task<bool> DeleteAsync(Guid id);
    }
}
