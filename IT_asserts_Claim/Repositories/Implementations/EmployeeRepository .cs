using IT_asserts.Repositories.Interface;
using IT_asserts_Claim.Data;
using IT_asserts_Claim.Models;
using Microsoft.EntityFrameworkCore;

namespace IT_asserts.Repositories.Implementations
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly AppDbContext _context;

        public EmployeeRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Employee>> GetAllAsync()
        {
            return await _context.Employees
                .AsNoTracking()
                .OrderBy(e => e.EmployeeName)
                .ToListAsync();
        }

        public async Task<Employee?> GetByIdAsync(Guid id)
        {
            return await _context.Employees.FindAsync(id);
        }
    }
}
