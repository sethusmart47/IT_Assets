using IT_asserts_Claim.Models;

namespace IT_asserts.Repositories.Interface
{
    public interface IEmployeeRepository
    {
        Task<List<Employee>> GetAllAsync();
        Task<Employee?> GetByIdAsync(Guid id);
    }
}
