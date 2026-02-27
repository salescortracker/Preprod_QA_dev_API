using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.DTOs
{
    public class WfhApprovalDto
    {
        public List<int> WFHRequestIDs { get; set; } = new();
        public string Status { get; set; }          // Approved / Rejected
        public int ManagerID { get; set; }
        public string? ManagerRemarks { get; set; }
    }
}
