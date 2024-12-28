using System;
using System.Collections.Generic;

namespace MedSys.BL.DALModels;

public partial class MedicalDocument
{
    public int Id { get; set; }

    public int CheckupId { get; set; }

    public string Title { get; set; } = null!;

    public string? Text { get; set; }

    public DateTime? CreatedAt { get; set; }

    public string? Notes { get; set; }

    public string? FileKey { get; set; }

    public virtual Checkup Checkup { get; set; } = null!;
}
