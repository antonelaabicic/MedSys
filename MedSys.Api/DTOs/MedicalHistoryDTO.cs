using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MedSys.Api.Dtos
{
    public class MedicalHistoryDTO
    {
        [JsonIgnore]
        public int Id { get; set; }

        [Required]
        public PatientDTO Patient { get; set; } = null!;

        [Required]
        public DiseaseDTO Disease { get; set; } = null!;

        [Required]
        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }
    }
}