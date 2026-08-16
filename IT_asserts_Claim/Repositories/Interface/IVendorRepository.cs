using IT_asserts_Claim.entity;

namespace IT_asserts_Claim.Repositories.Interface
{
    public interface IVendorRepository
    {
        Task<List<Vendor>> GetAllAsync();
        Task<Vendor?> GetByIdAsync(Guid id);
        Task<bool> IsNameExistsAsync(string vendorName, Guid? excludeId = null);
        Task<bool> IsEmailExistsAsync(string email, Guid? excludeId = null);
        Task<bool> IsGSTExistsAsync(string gstNumber, Guid? excludeId = null);
        Task AddAsync(Vendor entity);
        void Update(Vendor entity);
        Task<int> SaveChangesAsync();
    }
}
