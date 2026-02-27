using System;
using System.Collections.Generic;

namespace DataAccessLayer.DBContext;

public partial class Candidate
{
    public int CandidateId { get; set; }

    public int RegionId { get; set; }

    public int CompanyId { get; set; }

    public int UserId { get; set; }

    public string SeqNo { get; set; } = null!;

    public int StageId { get; set; }

    public DateOnly? AppliedDate { get; set; }

    public string FirstName { get; set; } = null!;

    public string? LastName { get; set; }

    public string Email { get; set; } = null!;

    public string? Mobile { get; set; }

    public string? Gender { get; set; }

    public DateOnly? DateOfBirth { get; set; }

    public string? MaritalStatus { get; set; }

    public decimal? CurrentSalary { get; set; }

    public decimal? ExpectedSalary { get; set; }

    public string? ReferenceSource { get; set; }

    public string? Department { get; set; }

    public string? Designation { get; set; }

    public string? Skills { get; set; }

    public string? NoticePeriod { get; set; }

    public string? AnyOffers { get; set; }

    public string? Location { get; set; }

    public string? Reason { get; set; }

    public string? FileName { get; set; }

    public string? FilePath { get; set; }

    public bool IsActive { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime CreatedAt { get; set; }

    public int? ModifiedBy { get; set; }

    public DateTime? ModifiedAt { get; set; }

    public string? CandidateName { get; set; }

    public string? Technology { get; set; }

    public int? ExperienceYears { get; set; }

    public string? CurrentCtc { get; set; }

    public int? CurrentStageId { get; set; }

    public int? ProgressPercent { get; set; }

    public virtual ICollection<CandidateExperience> CandidateExperiences { get; set; } = new List<CandidateExperience>();

    public virtual ICollection<CandidateInterview> CandidateInterviews { get; set; } = new List<CandidateInterview>();

    public virtual ICollection<CandidateOffer> CandidateOffers { get; set; } = new List<CandidateOffer>();

    public virtual ICollection<CandidateQualification> CandidateQualifications { get; set; } = new List<CandidateQualification>();

    public virtual ICollection<CandidateScreening> CandidateScreenings { get; set; } = new List<CandidateScreening>();

    public virtual StageMaster Stage { get; set; } = null!;
}
