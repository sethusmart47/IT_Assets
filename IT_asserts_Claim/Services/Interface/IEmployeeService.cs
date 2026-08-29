using IT_asserts.Dtos.Employee;

namespace IT_asserts.Services.Interface
{
    public interface IEmployeeService
    {
        Task<List<EmployeeDto>> GetAllEmployeesAsync();
        Task<EmployeeDto?> GetEmployeeDetailAsync(Guid id);
    }
}
