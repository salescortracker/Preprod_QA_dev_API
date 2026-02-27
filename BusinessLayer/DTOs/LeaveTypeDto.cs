using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.DTOs
{
    public class LeaveTypeDto
    {
        public int LeaveTypeID { get; set; }
        public int CompanyID { get; set; }
        public int RegionID { get; set; }

        public string LeaveTypeName { get; set; } = string.Empty;
        public string? Description { get; set; }

        public int LeaveDays { get; set; }
        public bool IsActive { get; set; }

        // ✅ DISPLAY FIELDS (LIKE KPI)
        public string CompanyName { get; set; } = string.Empty;
        public string RegionName { get; set; } = string.Empty;
    }
}
