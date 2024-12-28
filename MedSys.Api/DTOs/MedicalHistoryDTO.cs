using MedSys.Api.DTOs;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MedSys.Api.Dtos
{
    public class MedicalHistoryDTO
    {
        public int Id { get; set; }

        [Required]
        public PatientSimplifiedDTO Patient { get; set; } = null!;

        [Required]
        public DiseaseSimplifiedDTO Disease { get; set; } = null!;
        public int DiseaseId { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }
    }
}