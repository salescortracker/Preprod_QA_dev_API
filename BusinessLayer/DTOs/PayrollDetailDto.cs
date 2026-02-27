using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.DTOs
{
    public class PayrollDetailDto
    {
        public int ComponentId { get; set; }
        public string ComponentName { get; set; }
        public decimal Amount { get; set; }
        public string Type { get; set; }

        public int UserId { get; set; }
        public string? CompanyId { get; set; }
        public string? RegionId { get; set; }
    }
}
