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
    }
}
