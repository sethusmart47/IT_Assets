using AutoMapper;
using IT_asserts.Dtos.Employee;
using IT_asserts.Repositories.Interface;
using IT_asserts.Services.Interface;

namespace IT_asserts.Services.Implementations
{
    public class EmployeeService:IEmployeeService
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
        public async Task<List<EmployeeDto>> GetAllEmployeesAsync()
        {
            var employees = await _employeeRepository.GetAllAsync();
            var employeeDtos = _mapper.Map<List<EmployeeDto>>(employees);

            foreach (var dto in employeeDtos)
            {
                dto.ActiveAssetCount = await _assignmentRepository.GetActiveCountByEmployeeIdAsync(dto.Id);
            }

            return employeeDtos;
        }

        public async Task<EmployeeDto?> GetEmployeeDetailAsync(Guid id)
        {
            var employee = await _employeeRepository.GetByIdAsync(id);
            if (employee == null) return null;

            var detail = _mapper.Map<EmployeeDto>(employee);

            return detail;
        }

    }
}
