using ITAssetManagement.Repositories.Interface;
using ITAssetManagement.Data;
using ITAssetManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ITAssetManagement.Repositories.Implementations
{
    public class EmployeeRepository : Repository<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<List<Employee>> GetAllEmployeesAsync()
        {
            return await Context.Employees
                .AsNoTracking()
                .OrderBy(e => e.EmployeeName)
                .ToListAsync();
        }

        public async Task<Employee?> GetEmployeeByIdAsync(Guid id)
        {
            return await Context.Employees.FindAsync(id);
        }
    }
}