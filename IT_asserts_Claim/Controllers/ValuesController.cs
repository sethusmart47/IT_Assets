using IT_asserts_Claim.Data;
using IT_asserts_Claim.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IT_asserts_Claim.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly AppDbContext _context;
        public ValuesController(AppDbContext context)
        {
            _context = context;
        }
        [HttpPost("{empId}")]
        public async Task<IActionResult> AddAccessory(int empId, Accessory accessory)
        {
            var emp = await _context.Employees.FindAsync(empId);
            if (emp == null)
                return NotFound("Employee not found");

            accessory.EmpId = empId;
            _context.Accessories.Add(accessory);
            await _context.SaveChangesAsync();

            return Ok(accessory);
        }

        
        [HttpPut("{empcode}/{id}")]
        public async Task<IActionResult> UpdateAccessory(string empcode, int id, Accessory updated)
        {
            var existing = await _context.Accessories.FirstOrDefaultAsync(a => a.Id==id && a.Employee.EmpCode==empcode);
            if (existing == null)
                return NotFound();

            existing.AccessoryType = updated.AccessoryType;
            existing.AccessoryName = updated.AccessoryName;
            existing.SerialNo = updated.SerialNo;
            existing.IssueDate = updated.IssueDate;

            await _context.SaveChangesAsync();
            return Ok(existing);
        }


        //[HttpDelete("{empcode}/{id}")]
        //public async Task<IActionResult> DeleteAccessory(string empcode, int id)
        //{
        //    var accessory = await _context.Accessories
        //        .FirstOrDefaultAsync(a => a.Id == id && a.Employee.EmpCode == empcode);

        //    if (accessory == null)
        //        return NotFound("Accessory not found for this employee");

        //    _context.Accessories.Remove(accessory);
        //    await _context.SaveChangesAsync();

        //    return Ok("Accessory deleted successfully");
        //}
        [HttpDelete("{empcode}/{id}")]
        public async Task<IActionResult> DeleteAccessory(string empcode, int id)
        {
            // Find the employee by their code
            var employee = await _context.Employees
                .FirstOrDefaultAsync(e => e.EmpCode == empcode);

            if (employee == null)
                return NotFound("Employee not found");

            // Find the accessory belonging to that employee
            var accessory = await _context.Accessories
                .FirstOrDefaultAsync(a => a.Id == id && a.EmpId == employee.Id);

            if (accessory == null)
                return NotFound("Accessory not found for this employee");

            _context.Accessories.Remove(accessory);
            await _context.SaveChangesAsync();

            return Ok("Accessory deleted successfully");
        }


    }
}
