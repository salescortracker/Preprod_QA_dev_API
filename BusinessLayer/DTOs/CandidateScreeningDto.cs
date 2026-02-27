using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.DTOs
{
    public class CandidateScreeningDto
    {
        public int RegionId { get; set; }
        public int CompanyId { get; set; }
        public int UserId { get; set; }
        public int CandidateId { get; set; }
        public int RecruiterId { get; set; }
        public string ScreeningStatus { get; set; } = null!;
        public string? Remarks { get; set; }

        // 🔹 Added (used for READ / LISTING)
        public string? SeqNo { get; set; }
        public string? CandidateName { get; set; }
        public string? RecruiterName { get; set; }
        public DateTime? ScreeningDate { get; set; }
        public int StageId { get; set; }
        public string? Mobile { get; set; }
        public decimal? ExpectedSalary { get; set; }
    }
}
