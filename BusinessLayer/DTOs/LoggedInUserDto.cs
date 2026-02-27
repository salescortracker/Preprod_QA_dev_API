using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.DTOs
{
    public class LoggedInUserDto
    {
        public int UserId { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeCode { get; set; }
    }
}
