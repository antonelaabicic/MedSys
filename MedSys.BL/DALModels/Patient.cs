using System;
using System.Collections.Generic;

namespace MedSys.BL.DALModels;

public partial class Patient
{
    public int Id { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public DateOnly DateOfBirth { get; set; }

    public string? Gender { get; set; }

    public string Oib { get; set; } = null!;

    public virtual ICollection<Checkup> Checkups { get; set; } = new List<Checkup>();

    public virtual ICollection<MedicalHistory> MedicalHistories { get; set; } = new List<MedicalHistory>();

    public virtual ICollection<Prescription> Prescriptions { get; set; } = new List<Prescription>();
}
