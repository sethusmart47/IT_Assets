using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace IT_asserts_Claim.Models
{
    [Index(nameof(EmpCode), IsUnique = true)]
    public class Employee
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string EmpCode { get; set; }
        
        public string EmpName { get; set; }
        public string EmpMail { get; set; }

        public ICollection <Accessory> Accessories { get; set; }

    }
}
