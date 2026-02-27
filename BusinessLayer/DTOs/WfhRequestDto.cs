using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.DTOs
{
    public class WfhRequestDto
    {
        public int WFHRequestID { get; set; }
        public int EmployeeID { get; set; }
        public string EmployeeName { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string RequestType { get; set; }
        public string Reason { get; set; }
        public string Status { get; set; }
        public string? ManagerRemarks { get; set; }
    }
}
