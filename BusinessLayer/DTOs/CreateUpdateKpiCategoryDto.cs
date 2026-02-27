using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.DTOs
{
    public class CreateUpdateKpiCategoryDto
    {
        public int KpiCategoryID { get; set; } = 0;
        public string KpiCategoryName { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool IsActive { get; set; } = true;
        public int CompanyID { get; set; }
        public int RegionID { get; set; }
    }
}
