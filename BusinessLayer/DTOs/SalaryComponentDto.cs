using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.DTOs
{
    public class SalaryComponentDto
    {
        public int ComponentId { get; set; }
        public string ComponentName { get; set; }
        public string Type { get; set; } // Earning / Deduction
        public string CalculationType { get; set; } // Fixed / Percentage
        public string? PercentageOf { get; set; } // Basic / Gross
        public bool IsTaxable { get; set; }
        public bool IsActive { get; set; }

        public DateTime CreatedAt { get; set; }

        public string? CreatedBy { get; set; }

        public DateTime? ModifiedAt { get; set; }

        public string? ModifiedBy { get; set; }

        public int? UserId { get; set; }

        public string? CompanyId { get; set; }

        public string? RegionId { get; set; }
    }
}
