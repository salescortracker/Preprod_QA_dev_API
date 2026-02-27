using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.DTOs
{
    public class DepartmentDto
    {
        public int departmentId { get; set; }
        public string departmentName { get; set; }
        public int companyId { get; set; }
        public int regionId { get; set; }
        public string description { get; set; } = string.Empty;
        public bool isActive { get; set; }
        public int userId { get; set; }
    }

    // Application/DTOs/CreateUpdateDepartmentDto.cs
    public class CreateUpdateDepartmentDto
    {
        public int departmentId { get; set; }
        public int companyId { get; set; }
        public int regionId { get; set; }
        public string departmentName { get; set; }
        public string description { get; set; } = string.Empty;
        public bool isActive { get; set; } = true;
        public int userId { get; set; }
    }
}
