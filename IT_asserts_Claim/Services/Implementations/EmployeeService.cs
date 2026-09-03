using AutoMapper;
using ITAssetManagement.Domain.Entities;
using ITAssetManagement.Dtos.Employee;
using ITAssetManagement.Repositories.Interface;
using ITAssetManagement.Services.Interface;

namespace ITAssetManagement.Services.Implementations
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IAssetAssignmentRepository _assignmentRepository;
        private readonly IMapper _mapper;

        public EmployeeService(
            IEmployeeRepository employeeRepository,
            IAssetAssignmentRepository assignmentRepository,
            IMapper mapper)
        {
            _employeeRepository = employeeRepository;
            _assignmentRepository = assignmentRepository;
            _mapper = mapper;
        }

        public async Task<List<EmployeeDetails>> GetAllEmployeesAsync()
        {
            var employees = await _employeeRepository.GetAllEmployeesAsync();
            var employeeDtos = _mapper.Map<List<EmployeeDetails>>(employees);

            foreach (var dto in employeeDtos)
            {
                dto.ActiveAssetCount = await _assignmentRepository.GetActiveAssetAssignmentCountByEmployeeIdAsync(dto.Id);
            }

            return employeeDtos;
        }

        public async Task<EmployeeDetails?> GetEmployeeDetailAsync(Guid id)
        {
            var employee = await _employeeRepository.GetEmployeeByIdAsync(id);
            if (employee == null) return null;

            var dto = _mapper.Map<EmployeeDetails>(employee);
            dto.ActiveAssetCount = await _assignmentRepository.GetActiveAssetAssignmentCountByEmployeeIdAsync(id);
            return dto;
        }
    }
}
