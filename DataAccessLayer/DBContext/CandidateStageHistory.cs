using System;
using System.Collections.Generic;

namespace DataAccessLayer.DBContext;

public partial class CandidateStageHistory
{
    public int HistoryId { get; set; }

    public int CandidateId { get; set; }

    public int StageId { get; set; }

    public int ChangedBy { get; set; }

    public DateTime ChangedDate { get; set; }

    public string? Remarks { get; set; }

    public virtual Candidate Candidate { get; set; } = null!;

    public virtual User ChangedByNavigation { get; set; } = null!;

    public virtual RecruitmentStage Stage { get; set; } = null!;
}
