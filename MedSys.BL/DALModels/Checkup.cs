using System;
using System.Collections.Generic;

namespace MedSys.BL.DALModels;

public partial class Checkup
{
    public int Id { get; set; }

    public int PatientId { get; set; }

    public int CheckupTypeId { get; set; }

    public DateOnly Date { get; set; }

    public TimeOnly Time { get; set; }

    public virtual CheckupType CheckupType { get; set; } = null!;

    public virtual ICollection<MedicalDocument> MedicalDocuments { get; set; } = new List<MedicalDocument>();

    public virtual Patient Patient { get; set; } = null!;
}
