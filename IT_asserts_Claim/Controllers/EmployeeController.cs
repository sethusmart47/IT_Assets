using IT_asserts_Claim.Data;
using IT_asserts_Claim.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IT_asserts_Claim.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly AppDbContext _context;
        public EmployeeController( AppDbContext context) { 
        
        _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> PostEmployee([FromBody] Employee employee)
        {
            if (employee == null)
                return BadRequest("Employee data is null");

            // Link accessories to employee
            if (employee.Accessories != null)
            {
                foreach (var accessory in employee.Accessories)
                {
                    accessory.Employee = employee;
                }
            }

            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();
            return Ok(employee);
        }

        [HttpGet]
        public async Task<IActionResult> GetEmployees()
        {
            var employees = _context.Employees.ToList();
            return Ok(employees);
        }

        [HttpGet]
        [Route("{Empcode}")]

        public async Task<IActionResult> GetEmployeeById(string Empcode)
        {
            var employee = _context.Employees.Find(Empcode);
            if (employee == null)
            {
                return NotFound();
            }
            return Ok(employee);
        }
        [HttpGet("{empCode}")]
        public async Task<IActionResult> GetByEmpCode(string empCode)
        {
            var employee = await _context.Employees
                .Include(e => e.Accessories)
                .FirstOrDefaultAsync(e => e.EmpCode == empCode);

            if (employee == null)
                return NotFound();

            return Ok(employee);
        }

        [HttpGet("id/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var employee = await _context.Employees
                .Include(e => e.Accessories)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (employee == null)
                return NotFound();

            return Ok(employee);
        }

    }

}
