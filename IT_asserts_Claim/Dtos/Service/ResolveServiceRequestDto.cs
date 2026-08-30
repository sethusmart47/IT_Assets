using System.ComponentModel.DataAnnotations;

namespace IT_asserts.Dtos.Service
{
   
     public class ResolveServiceRequestDto
    {
        [MaxLength(2000)]
        public string? ResolutionNotes { get; set; }
    }
}
