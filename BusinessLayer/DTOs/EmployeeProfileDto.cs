using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.DTOs
{
    public class EmployeeProfileDto
    {
        public string EmployeeCode { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }

        public string BandGrade { get; set; }
        public string EsicNumber { get; set; }
        public string PFNumber { get; set; }
        public string UAN { get; set; }

        public string ReportingManager { get; set; }
        public string Rolename { get; set; }
        public DateTime? DateOfJoining { get; set; }
        public string EmployeeType { get; set; }

        public string ServiceStatus { get; set; }
        public string Location { get; set; }

        public string ShiftName { get; set; }
        public string SkypeId { get; set; }
    }
}
