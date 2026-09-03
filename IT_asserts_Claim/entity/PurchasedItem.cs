using ITAssetManagement.Domain.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ITAssetManagement.Domain.Entities
{
    public class PurchasedItem:AuditInfo
    {
        

        [Required]
        [MaxLength(100)]
        public string Category { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string Brand { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string Model { get; set; } = string.Empty;

        [Required]
        [MaxLength(300)]
        public string Configuration { get; set; } = string.Empty;

        [Required]
        public int Quantity { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal UnitPrice { get; set; }

       // [MaxLength(50)]
        //public string WarrantyPeriod { get; set; } = string.Empty;

        [Column(TypeName = "decimal(18,2)")]
        public decimal SubTotal { get; set; }
        [Required]
        public Guid PurchaseId { get; set; }

        
        [ForeignKey(nameof(PurchaseId))]
        public Purchase Purchase { get; set; } = null!;
    }
}
