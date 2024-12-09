using MedSys.Api.DTOs;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MedSys.Api.Dtos
{
    public class CheckupDTO
    {
        public int Id { get; set; }

        //[Required]
        //public PatientSimplifiedDTO Patient { get; set; } = null!;

        [Required]
        public CheckupTypeDTO CheckupType { get; set; } = null!;

        [Required]
        public DateTime CheckupDateTime { get; set; }

        //public IEnumerable<MedicalDocumentDTO> MedicalDocuments { get; set; } = null!;
    }
}