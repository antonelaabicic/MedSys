using System;
using System.Collections.Generic;

namespace MedSys.BL.DALModels;

public partial class Disease
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<MedicalHistory> MedicalHistories { get; set; } = new List<MedicalHistory>();
}
