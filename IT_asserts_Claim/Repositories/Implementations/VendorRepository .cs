using IT_asserts_Claim.Data;
using IT_asserts_Claim.entity;
using IT_asserts_Claim.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace IT_asserts_Claim.Repositories.Implementations
{
    public class VendorRepository : IVendorRepository
    {
        private readonly AppDbContext _context;

        public VendorRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Vendor>> GetAllAsync()
        {
            return await _context.Vendors
                .OrderBy(x => x.VendorName)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Vendor?> GetByIdAsync(Guid id)
        {
            return await _context.Vendors
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<bool> IsNameExistsAsync(string vendorName, Guid? excludeId = null)
        {
            return await _context.Vendors
                .AnyAsync(x => x.VendorName.ToLower() == vendorName.ToLower()
                    && (!excludeId.HasValue || x.Id != excludeId.Value));
        }

        public async Task<bool> IsEmailExistsAsync(string email, Guid? excludeId = null)
        {
            return await _context.Vendors
                .AnyAsync(x => x.Email.ToLower() == email.ToLower()
                    && (!excludeId.HasValue || x.Id != excludeId.Value));
        }

        public async Task<bool> IsGSTExistsAsync(string gstNumber, Guid? excludeId = null)
        {
            return await _context.Vendors
                .AnyAsync(x => x.GSTNumber != null
                    && x.GSTNumber.ToLower() == gstNumber.ToLower()
                    && (!excludeId.HasValue || x.Id != excludeId.Value));
        }

        public async Task AddAsync(Vendor entity)
        {
            await _context.Vendors.AddAsync(entity);
        }

        public void Update(Vendor entity)
        {
            _context.Vendors.Update(entity);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}