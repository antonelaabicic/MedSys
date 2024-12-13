using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MedSys.Api.DTOs
{
    public class DiseaseSimplifiedDTO
    {
        [JsonIgnore]
        public int Id { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Disease name can't be longer than 100 characters.")]
        public string Name { get; set; } = null!;
    }
}
