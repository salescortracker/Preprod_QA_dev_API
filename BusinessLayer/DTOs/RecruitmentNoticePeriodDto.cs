using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.DTOs
{
    public class RecruitmentNoticePeriodDto
    {
        public int RecruitmentNoticePeriodID { get; set; }
        public int CompanyID { get; set; }
        public int RegionID { get; set; }
        public string NoticePeriod { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public int? UserId { get; set; }
    }
}
