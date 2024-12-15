using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MedSys.Api.DTOs
{
    public class CheckupSimplifiedDTO
    {
        [JsonIgnore]
        public int Id { get; set; }

        [Required]
        public PatientSimplifiedDTO Patient { get; set; } = null!;

        [Required]
        public CheckupTypeSimplifiedDTO CheckupType { get; set; } = null!;

        [Required]
        public DateTime CheckupDateTime { get; set; }
    }
}
