using MedSys.Api.Dtos;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MedSys.Api.DTOs
{
    public class CheckupTypeSimplifiedDTO
    {
        //[JsonIgnore]
        public int Id { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Name can't be longer than 100 characters.")]
        public string Name { get; set; } = null!;

        [Required]
        [StringLength(20, ErrorMessage = "Code can't be longer than 20 characters.")]
        public string Code { get; set; } = null!;
    }
}
