using System;
using System.Collections.Generic;

namespace DataAccessLayer.DBContext;

public partial class CandidateScreening
{
    public int ScreeningId { get; set; }

    public int RegionId { get; set; }

    public int CompanyId { get; set; }

    public int UserId { get; set; }

    public int CandidateId { get; set; }

    public int RecruiterId { get; set; }

    public string ScreeningStatus { get; set; } = null!;

    public string? Remarks { get; set; }

    public DateTime ScreeningDate { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime CreatedAt { get; set; }

    public int? ModifiedBy { get; set; }

    public DateTime? ModifiedAt { get; set; }

    public virtual Candidate Candidate { get; set; } = null!;
}
