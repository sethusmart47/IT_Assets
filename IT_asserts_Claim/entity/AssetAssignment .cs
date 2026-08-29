using Claim_Form.Entities;
using IT_asserts_Claim.entity;
using IT_asserts_Claim.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IT_asserts.entity
{
    
        public class AssetAssignment : AuditInfo
        {
            [Required]
            public Guid AssetId { get; set; }

            [Required]
            public Guid EmployeeId { get; set; }

            public DateTime AssignedDate { get; set; } = DateTime.UtcNow;

            public DateTime? ReturnedDate { get; set; }

            public int? ConditionAtReturn { get; set; }     // 1=Good, 2=Damaged, 3=Lost

            [MaxLength(500)]
            public string? Remarks { get; set; }

            // Navigation
            [ForeignKey(nameof(AssetId))]
            public Asset Asset { get; set; } = null!;

            [ForeignKey(nameof(EmployeeId))]
            public Employee Employee { get; set; } = null!;
        }
    }
    
