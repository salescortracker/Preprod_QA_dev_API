using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.DTOs
{
    public class BulkApproveRejectPunchDto
    {
        public List<int> MissedPunchRequestIds { get; set; } = new();
        public string Status { get; set; } // Approved / Rejected
        public string? ManagerRemarks { get; set; }
        public int ManagerID { get; set; }

        public int CompanyID { get; set; }
        public int? RegionID { get; set; }
    }
}
