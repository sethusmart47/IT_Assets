using ITAssetManagement.Domain.Entities;

namespace ITAssetManagement.Repositories.Interface
{
    public interface IVendorRepository
    {
        Task<List<Vendor>> GetAllVendorsAsync();
        Task<Vendor?> GetVendorByIdAsync(Guid id);
        Task<bool> IsNameExistsAsync(string vendorName, Guid? excludeId = null);
        Task<bool> IsEmailExistsAsync(string email, Guid? excludeId = null);
        Task<bool> IsGSTExistsAsync(string gstNumber, Guid? excludeId = null);
        Task AddVendorAsync(Vendor entity);
        void UpdateVendor(Vendor entity);
        Task<int> SaveChangesAsync();

        // Generic wrappers for backward compatibility
        Task<List<Vendor>> GetAllAsync();
        Task<Vendor?> GetByIdAsync(Guid id);
        Task AddAsync(Vendor entity);
        void Update(Vendor entity);
    }
}