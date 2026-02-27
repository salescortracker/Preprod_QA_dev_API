using System;
using System.Collections.Generic;

namespace DataAccessLayer.DBContext;

public partial class CandidateInterview
{
    public int InterviewId { get; set; }

    public int RegionId { get; set; }

    public int CompanyId { get; set; }

    public int UserId { get; set; }

    public int CandidateId { get; set; }

    public int LevelNo { get; set; }

    public int InterviewerId { get; set; }

    public string InterviewerName { get; set; } = null!;

    public DateTime InterviewDate { get; set; }

    public string? Location { get; set; }

    public string? MeetingLink { get; set; }

    public string? Description { get; set; }

    public string Result { get; set; } = null!;

    public int? CreatedBy { get; set; }

    public DateTime CreatedAt { get; set; }

    public int? ModifiedBy { get; set; }

    public DateTime? ModifiedAt { get; set; }

    public virtual Candidate Candidate { get; set; } = null!;
}
