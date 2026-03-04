using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.DTOs
{
    public class EmployeeDto
    {
        public int UserId { get; set; }       // Employee's unique ID
        public string FullName { get; set; }  // Employee's full names
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Rolename { get; set; }
        public string EmployeeType { get; set; }
        public string Status { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeCode { get; set; }
    }
}
