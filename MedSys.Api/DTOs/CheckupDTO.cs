using MedSys.Api.DTOs;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MedSys.Api.Dtos
{
    public class CheckupDTO
    {
        [JsonIgnore]
        public int Id { get; set; }

        [Required]
        public PatientDTO Patient { get; set; } = null!;

        [Required]
        public CheckupTypeDTO CheckupType { get; set; } = null!;

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public TimeSpan Time { get; set; }

        public IEnumerable<MedicalDocumentDTO> MedicalDocuments { get; set; } = null!;
    }
}