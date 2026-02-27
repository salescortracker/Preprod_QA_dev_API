using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.DTOs
{
    public class MissedPunchApprovalDto
    {
        public int MissedPunchRequestID { get; set; }
        public string Status { get; set; }   // Approved / Rejected
        public string? ManagerRemarks { get; set; }
        public int ManagerID { get; set; }
    }
}
