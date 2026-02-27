using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.DTOs
{
    public class UpdateResignationStatusRequest
    {
      
        public int ResignationId { get; set; }
        public string Status { get; set; }
        public string? ManagerReason { get; set; }
        public bool IsManagerApprove { get; set; }
        public bool IsManagerReject { get; set; }
        // ✅ ADD THESE
        public string? HRReason { get; set; }
        public bool IsHRApprove { get; set; }
        public bool IsHRReject { get; set; }
    }
}
