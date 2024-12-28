using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MedSys.Api.DTOs
{
    public class CheckupSimplifiedDTO
    {
        public int Id { get; set; }

        [Required]
        public int PatientId { get; set; } 

        [Required]
        public int CheckupTypeId { get; set; } 

        [Required]
        public DateTime CheckupDateTime { get; set; }
    }
}
