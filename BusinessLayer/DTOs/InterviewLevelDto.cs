using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.DTOs
{
    public class InterviewLevelDto
    {
        public int InterviewLevelsID { get; set; }
        public int? UserId { get; set; }
        public int CompanyID { get; set; }
        public int RegionID { get; set; }
        public string? InterviewLevels { get; set; }
        public bool IsActive { get; set; }
    }
}
