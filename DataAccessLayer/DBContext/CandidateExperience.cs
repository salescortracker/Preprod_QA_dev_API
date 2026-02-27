using System;
using System.Collections.Generic;

namespace DataAccessLayer.DBContext;

public partial class CandidateExperience
{
    public int ExperienceId { get; set; }

    public int CandidateId { get; set; }

    public int RegionId { get; set; }

    public int CompanyId { get; set; }

    public int UserId { get; set; }

    public int FromYear { get; set; }

    public int ToYear { get; set; }

    public string Designation { get; set; } = null!;

    public string Organization { get; set; } = null!;

    public int? CreatedBy { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual Candidate Candidate { get; set; } = null!;
}
