using System;
using System.Collections.Generic;

namespace DataAccessLayer.DBContext;

public partial class StageMaster
{
    public int StageId { get; set; }

    public string StageName { get; set; } = null!;

    public int StageOrder { get; set; }

    public int ProgressPct { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual ICollection<Candidate> Candidates { get; set; } = new List<Candidate>();
}
