using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.DTOs
{
    public class CandidateListDto
    {
        public int CandidateId { get; set; }
        public string CandidateName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Technology { get; set; } = null!;
        public int ExperienceYears { get; set; }
        public DateTime AppliedDate { get; set; }
        public string StageName { get; set; } = null!;
        public int ProgressPercent { get; set; }
        public string? FileName { get; set; }
        public string? FilePath { get; set; }
    }
}
