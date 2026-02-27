using System;
using System.Collections.Generic;

namespace DataAccessLayer.DBContext;

public partial class CandidateOnboarding
{
    public int OnboardingId { get; set; }

    public int RegionId { get; set; }

    public int CompanyId { get; set; }

    public int UserId { get; set; }

    public int CandidateId { get; set; }

    public DateOnly? JoiningDate { get; set; }

    public bool DocumentsCollected { get; set; }

    public string BackgroundCheckStatus { get; set; } = null!;

    public bool LaptopIssued { get; set; }

    public string? BuddyAssigned { get; set; }

    public string OnboardingStatus { get; set; } = null!;

    public int? CreatedBy { get; set; }

    public DateTime CreatedAt { get; set; }

    public int? ModifiedBy { get; set; }

    public DateTime? ModifiedAt { get; set; }

    public virtual Candidate Candidate { get; set; } = null!;
}
