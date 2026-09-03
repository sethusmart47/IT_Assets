using ITAssetManagement.Dtos.Employee;

namespace ITAssetManagement.Services.Interface
{
    public interface IEmployeeService
    {
        Task<List<EmployeeDetails>> GetAllEmployeesAsync();
        Task<EmployeeDetails?> GetEmployeeDetailAsync(Guid id);
    }
}
