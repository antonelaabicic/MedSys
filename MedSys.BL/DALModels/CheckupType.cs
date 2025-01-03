﻿using System;
using System.Collections.Generic;

namespace MedSys.BL.DALModels;

public partial class CheckupType
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Code { get; set; } = null!;

    public virtual ICollection<Checkup> Checkups { get; set; } = new List<Checkup>();
}
