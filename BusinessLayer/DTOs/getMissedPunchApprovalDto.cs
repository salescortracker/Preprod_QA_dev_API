using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.DTOs
{
    public class getMissedPunchApprovalDto
    {
        public int RequestId { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }   // 👈 ADD THIS
        public DateOnly MissedDate { get; set; }
        public string MissedType { get; set; }
        public string Reason { get; set; }
        public int clockIn { get; set; }
    }
}
