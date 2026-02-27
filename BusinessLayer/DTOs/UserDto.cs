using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.DTOs
{
    public class UserCreateDto
    {
        public int userId { get; set; }
        public int CompanyID { get; set; }
        public int RegionID { get; set; }
        public string EmployeeCode { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty; // plain text for creation (hashed before save)
        public int RoleId { get; set; }
        public int reportingTo { get; set; }
        public int departmentId { get; set; }
        public int? UserCompanyId { get; set; }
    }

    public class UserUpdateDto
    {
        public int UserID { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public int RoleId { get; set; }
        public int reportingTo { get; set; }
        public int departmentId { get; set; }
        public string Status { get; set; } = "Active"; // or "Inactive"
    }

    public class UserReadDto
    {
        public int UserID { get; set; }
        public string EmployeeCode { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string RoleName { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }
        public int? CompanyID { get; set; }
        public int? RegionID { get; set; }
    }
}
