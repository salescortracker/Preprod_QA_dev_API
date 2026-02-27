using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.DTOs
{
    public class ResignationDto
    {
        public int ResignationId { get; set; }
        public int CompanyId { get; set; }
        public int RegionId { get; set; }

        public string ResignationType { get; set; } = string.Empty;
        public int NoticePeriodDays { get; set; }

        public bool IsActive { get; set; }
    }
}
