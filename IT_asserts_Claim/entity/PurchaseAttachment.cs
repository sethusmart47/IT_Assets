using ITAssetManagement.Domain.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ITAssetManagement.Domain.Entities
{
    public class PurchaseAttachment:AuditInfo
    {
        

        [Required]
        [MaxLength(255)]
        public string FileName { get; set; } = string.Empty;

        [Required]
        [MaxLength(500)]
        public string FilePath { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string ContentType { get; set; } = string.Empty;

        public long FileSize { get; set; }
        [Required]
        public Guid PurchaseId { get; set; }

        // ─── Navigation ───
        [ForeignKey(nameof(PurchaseId))]
        public Purchase Purchase { get; set; } = null!;

    }
}
