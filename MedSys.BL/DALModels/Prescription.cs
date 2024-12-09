using System;
using System.Collections.Generic;

namespace MedSys.BL.DALModels;

public partial class Prescription
{
    public int Id { get; set; }

    public int PatientId { get; set; }

    public int DrugId { get; set; }

    public string Dosage { get; set; } = null!;

    public string Frequency { get; set; } = null!;

    public string Duration { get; set; } = null!;

    public DateOnly IssueDate { get; set; }

    public virtual Drug Drug { get; set; } = null!;

    public virtual Patient Patient { get; set; } = null!;
}
