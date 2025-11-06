using IT_asserts_Claim.Data;
using IT_asserts_Claim.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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

        
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAccessory(int id, Accessory updated)
        {
            var existing = await _context.Accessories.FindAsync(id);
            if (existing == null)
                return NotFound();

            existing.AccessoryType = updated.AccessoryType;
            existing.AccessoryName = updated.AccessoryName;
            existing.SerialNo = updated.SerialNo;
            existing.IssueDate = updated.IssueDate;

            await _context.SaveChangesAsync();
            return Ok(existing);
        }

        
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAccessory(int id)
        {
            var acc = await _context.Accessories.FindAsync(id);
            if (acc == null) return NotFound();

            _context.Accessories.Remove(acc);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
