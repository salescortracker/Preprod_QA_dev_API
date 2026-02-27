using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.DTOs
{
    public class CandidateDto
    {
        public int CandidateId { get; set; }
        public int RegionId { get; set; }
        public int CompanyId { get; set; }
        public int UserId { get; set; }

        public string SeqNo { get; set; } = null!;
        public int StageId { get; set; }

        public DateTime? AppliedDate { get; set; }

        public string FirstName { get; set; } = null!;
        public string? LastName { get; set; }
        public string Email { get; set; } = null!;
        public string? Mobile { get; set; }

        public string? Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
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

        public IFormFile? ResumeFile { get; set; }
        public string? FileName { get; set; }
        public string? FilePath { get; set; }
        public string? ExperiencesJson { get; set; }
        public string? QualificationsJson { get; set; }


        public List<CandidateExperienceDto> Experiences { get; set; } = new();
        public List<CandidateQualificationDto> Qualifications { get; set; } = new();
    }
}
