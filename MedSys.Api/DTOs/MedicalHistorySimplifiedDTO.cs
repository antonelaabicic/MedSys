using MedSys.Api.DTOs;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MedSys.Api.Dtos
{
    public class MedicalHistorySimplifiedDTO
    {
        public int Id { get; set; }

        [Required]
        public int PatientId { get; set; } 

        [Required]
        public int DiseaseId { get; set; } 

        [Required]
        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }
    }
}