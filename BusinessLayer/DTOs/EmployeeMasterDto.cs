using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.DTOs
{
    public class EmployeeMasterDto
    {
        public int EmployeeMasterId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Role { get; set; }
        public int? RoleId { get; set; }
        public string? Department { get; set; }
        public int? ManagerId { get; set; }
        public int? CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
    }

    // BusinessLayer/DTOs/ManagerDropdownDto.cs
    public class ManagerDropdownDto
    {
        public int UserId { get; set; }
        public string FullName { get; set; } = string.Empty;
    }
}
