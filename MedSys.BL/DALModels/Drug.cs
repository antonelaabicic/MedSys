﻿using System;
using System.Collections.Generic;

namespace MedSys.BL.DALModels;

public partial class Drug
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Prescription> Prescriptions { get; set; } = new List<Prescription>();
}
