using System;
using System.Collections.Generic;

namespace DataAccessLayer.DBContext;

public partial class RecruitmentStage
{
    public int StageId { get; set; }

    public string StageName { get; set; } = null!;

    public int StageOrder { get; set; }

    public int ProgressPercent { get; set; }

    public virtual ICollection<CandidateStageHistory> CandidateStageHistories { get; set; } = new List<CandidateStageHistory>();

    public virtual ICollection<Candidate> Candidates { get; set; } = new List<Candidate>();
}
