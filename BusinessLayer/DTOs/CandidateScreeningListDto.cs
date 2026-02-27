using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.DTOs
{
    public class CandidateScreeningListDto
    {
        public int ScreeningId { get; set; }
        public string CandidateName { get; set; } = null!;
        public string Recruiters { get; set; } = null!;
        public string Result { get; set; } = null!;
        public string? Remarks { get; set; }
        public DateTime CreatedAt { get; set; }
        public int ProgressPercent { get; set; }
    }
}
