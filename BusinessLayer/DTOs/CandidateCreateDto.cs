using Microsoft.AspNetCore.Http;

namespace BusinessLayer.DTOs
{
    public class CandidateCreateDto
    {
        public int RegionId { get; set; }
        public int CompanyId { get; set; }
        public int UserId { get; set; }

        public string CandidateName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? Mobile { get; set; }
        public string Technology { get; set; } = null!;
        public int ExperienceYears { get; set; }
        public string? CurrentCTC { get; set; }

        public DateTime AppliedDate { get; set; }

        public IFormFile? ResumeFile { get; set; }

        public string? FileName { get; set; }
        public string? FilePath { get; set; }
    }
}
