using MedSys.Api.DTOs;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MedSys.Api.Dtos
{
    public class PrescriptionDTO
    {
        public int Id { get; set; }

        [Required]
        public PatientSimplifiedDTO Patient { get; set; } = null!;

        [Required]
        public DrugSimplifiedDTO Drug { get; set; } = null!;
        public int DrugId { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Dosage can't be longer than 100 characters.")]
        public string Dosage { get; set; } = null!;

        [Required]
        [StringLength(100, ErrorMessage = "Frequency can't be longer than 100 characters.")]
        public string Frequency { get; set; } = null!;

        [Required]
        [StringLength(100, ErrorMessage = "Duration can't be longer than 100 characters.")]
        public string Duration { get; set; } = null!;

        [Required]
        public DateTime IssueDate { get; set; }
    }
}
