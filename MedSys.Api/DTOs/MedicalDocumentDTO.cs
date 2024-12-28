using MedSys.Api.Dtos;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MedSys.Api.DTOs
{
    public class MedicalDocumentDTO
    {
        public int Id { get; set; }

        [Required]
        public int CheckupId { get; set; }

        [Required]
        [StringLength(200, ErrorMessage = "Title can't be longer than 200 characters.")]
        public string Title { get; set; } = null!;

        [Required]
        public string? Text { get; set; }

        public DateTime? CreatedAt { get; set; }

        public string? Notes { get; set; }

        public string? FileUrl { get; set; } = null!;

        [JsonIgnore] 
        public string? FileName { get; set; } 

        [JsonIgnore]
        public byte[]? FileContent { get; set; }
    }
}
