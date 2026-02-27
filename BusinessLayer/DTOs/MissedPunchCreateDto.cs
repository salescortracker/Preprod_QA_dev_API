using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.DTOs
{
    public class MissedPunchCreateDto
    {
        public int EmployeeID { get; set; }
        public DateOnly MissedDate { get; set; }
        public string MissedType { get; set; }
        public TimeOnly? CorrectClockIn { get; set; }
        public TimeOnly? CorrectClockOut { get; set; }
        public string Reason { get; set; }
        public int CompanyID { get; set; }
        public int UserId { get; set; }
        public int? MissedTypeID { get; set; }
    }
}
