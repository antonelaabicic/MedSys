﻿using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MedSys.Api.Dtos
{
    public class PatientDTO
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "First name can't be longer than 100 characters.")]
        public string FirstName { get; set; } = null!;

        [Required]
        [StringLength(100, ErrorMessage = "Last name can't be longer than 100 characters.")]
        public string LastName { get; set; } = null!;

        [Required]
        public DateTime DateOfBirth { get; set; }

        [StringLength(10, ErrorMessage = "Gender can't be longer than 10 characters.")]
        public string? Gender { get; set; }

        [Required]
        [StringLength(20, ErrorMessage = "OIB can't be longer than 20 characters.")]
        public string Oib { get; set; } = null!;
    }
}