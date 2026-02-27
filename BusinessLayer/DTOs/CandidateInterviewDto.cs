using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.DTOs
{
    public class CandidateInterviewDto
    {
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

        public string Result { get; set; } = "Pending";

        // 🔽 Display fields
        public string? SeqNo { get; set; }
        public string? CandidateName { get; set; }
        public string? Mobile { get; set; }
        public int StageId { get; set; }
        public decimal? ExpectedSalary { get; set; }
        public string? Department { get; internal set; }
        public string? Designation { get; internal set; }
    }
}
