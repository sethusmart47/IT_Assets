using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
namespace IT_asserts_Claim.Models
{
    public class Employee
    {
        [Key]
        public int Id { get; set; }
     
        public string EmpCode { get; set; }
        public string EmpName { get; set; }
        public string EmpMail { get; set; }

        public ICollection <Accessory> Accessories { get; set; }

    }
}
