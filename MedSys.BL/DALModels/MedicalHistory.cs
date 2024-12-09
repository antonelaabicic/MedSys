using System;
using System.Collections.Generic;

namespace MedSys.BL.DALModels;

public partial class MedicalHistory
{
    public int Id { get; set; }

    public int PatientId { get; set; }

    public int DiseaseId { get; set; }

    public DateOnly StartDate { get; set; }

    public DateOnly? EndDate { get; set; }

    public virtual Disease Disease { get; set; } = null!;

    public virtual Patient Patient { get; set; } = null!;
}
