using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.DTOs
{
    public class WfhRequestCreateDto
    {
        public int EmployeeID { get; set; }
        public string EmployeeName { get; set; }
        public DateOnly FromDate { get; set; }
        public DateOnly ToDate { get; set; }
        public string RequestType { get; set; }   // Work from Home / Remote Work
        public string Reason { get; set; }
        public string? DocumentPath { get; set; }
        public int ManagerID { get; set; }
        public int CompanyID { get; set; }
        public int? RegionID { get; set; }
        public int UserId { get; set; }
    }
}
