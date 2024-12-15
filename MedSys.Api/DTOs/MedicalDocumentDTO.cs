using MedSys.Api.Dtos;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MedSys.Api.DTOs
{
    public class MedicalDocumentDTO
    {
        public int Id { get; set; }

        [Required]
        public CheckupSimplifiedDTO Checkup { get; set; } = null!;

        [Required]
        [StringLength(200, ErrorMessage = "Title can't be longer than 200 characters.")]
        public string Title { get; set; } = null!;

        [Required]
        public string? Text { get; set; }

        [JsonIgnore]
        public DateTime? CreatedAt { get; set; }

        public string? Notes { get; set; }

        [JsonIgnore]
        public string? FileData { get; set; } = null!;

        [JsonIgnore]
        public string? FilePath { get; set; } = null!;
    }
}
