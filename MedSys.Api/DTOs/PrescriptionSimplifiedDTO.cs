using MedSys.Api.DTOs;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MedSys.Api.Dtos
{
    public class PrescriptionSimplifiedDTO
    {
        public int Id { get; set; }

        [Required]
        public int PatientId { get; set; }

        [Required]
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
