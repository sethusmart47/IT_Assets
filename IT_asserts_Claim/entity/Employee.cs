using ITAssetManagement.Domain.Entities;
using System.ComponentModel.DataAnnotations;
namespace ITAssetManagement.Domain.Entities
{

    public class Employee : AuditInfo
    {
        [Required]
        [MaxLength(150)]
        public string EmployeeName { get; set; } = string.Empty;

        [Required]
        [MaxLength(150)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string Department { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string Designation { get; set; } = string.Empty;
    }
}
