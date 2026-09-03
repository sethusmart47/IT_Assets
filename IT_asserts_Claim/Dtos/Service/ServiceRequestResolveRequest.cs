using System.ComponentModel.DataAnnotations;

namespace ITAssetManagement.Dtos.Service
{
   
     public class ServiceRequestResolveRequest
    {
        [MaxLength(2000)]
        public string? ResolutionNotes { get; set; }
    }
}
