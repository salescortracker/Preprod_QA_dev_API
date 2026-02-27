using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.DTOs
{
    public class MyTeamDto
    {
        public int EmployeeMasterId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string? Role { get; set; }
        public int? ManagerId { get; set; }

        public List<MyTeamDto> Subordinates { get; set; } = new List<MyTeamDto>();
    }
}
