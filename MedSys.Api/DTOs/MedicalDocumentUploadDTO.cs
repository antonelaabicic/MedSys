using System.ComponentModel.DataAnnotations;

namespace MedSys.Api.DTOs
{
    public class MedicalDocumentUploadDTO
    {
        //[Required]
        //public int CheckupId { get; set; }

        //[Required]
        //[StringLength(200, ErrorMessage = "Title can't be longer than 200 characters.")]
        //public string Title { get; set; } = null!;

        //[Required]
        //public string? Text { get; set; }

        //public DateTime? CreatedAt { get; set; }

        //public string? Notes { get; set; }

        [Required]
        public IFormFile MedicalDocument { get; set; } = null!;
    }
}
