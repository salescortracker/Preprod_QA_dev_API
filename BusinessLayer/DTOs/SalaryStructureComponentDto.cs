using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.DTOs
{
    public class SalaryStructureComponentDto
    {
        public int ComponentId { get; set; }
        public decimal Value { get; set; }  // ✅ matches DB

        public string CalculationType { get; set; } = string.Empty; // Fixed / Percentage
        public int StructureId { get; set; }

        public DateTime CreatedAt { get; set; }

        public string? CreatedBy { get; set; }

        public DateTime? ModifiedAt { get; set; }

        public string? ModifiedBy { get; set; }

        public int? UserId { get; set; }

        public string? CompanyId { get; set; }

        public string? RegionId { get; set; }
    }
}
