using ITAssetManagement.Data;
using ITAssetManagement.Domain.Entities;
using ITAssetManagement.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace ITAssetManagement.Repositories.Implementations
{
    public class VendorRepository : Repository<Vendor>, IVendorRepository
    {
        public VendorRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<List<Vendor>> GetAllVendorsAsync()
        {
            return await Context.Vendors
                .OrderBy(x => x.VendorName)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Vendor?> GetVendorByIdAsync(Guid id)
        {
            return await Context.Vendors
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<bool> IsNameExistsAsync(string vendorName, Guid? excludeId = null)
        {
            return await Context.Vendors
                .AnyAsync(x => x.VendorName.ToLower() == vendorName.ToLower()
                    && (!excludeId.HasValue || x.Id != excludeId.Value));
        }

        public async Task<bool> IsEmailExistsAsync(string email, Guid? excludeId = null)
        {
            return await Context.Vendors
                .AnyAsync(x => x.Email.ToLower() == email.ToLower()
                    && (!excludeId.HasValue || x.Id != excludeId.Value));
        }

        public async Task<bool> IsGSTExistsAsync(string gstNumber, Guid? excludeId = null)
        {
            return await Context.Vendors
                .AnyAsync(x => x.GSTNumber != null
                    && x.GSTNumber.ToLower() == gstNumber.ToLower()
                    && (!excludeId.HasValue || x.Id != excludeId.Value));
        }

        public async Task AddVendorAsync(Vendor entity) => await AddAsync(entity);

        public void UpdateVendor(Vendor entity) => Update(entity);

        public async Task<int> SaveChangesAsync() => await Context.SaveChangesAsync();
    }
}