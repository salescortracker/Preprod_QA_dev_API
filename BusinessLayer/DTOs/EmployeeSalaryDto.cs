using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.DTOs
{
    public class EmployeeSalaryDto
    {
        public int EmployeeSalaryId { get; set; }

        public int EmployeeId { get; set; }

        public int StructureId { get; set; }

        public DateOnly EffectiveFrom { get; set; }

        public decimal CTC { get; set; }

        public bool IsActive { get; set; }

        public string? CompanyId { get; set; }

        public string? RegionId { get; set; }

        public int? UserId { get; set; }

        public DateTime? CreatedAt { get; set; }

        public DateTime? ModifiedAt { get; set; }
        public string? EmployeeName { get; set; }
        public string? StructureName { get; set; }
    }
}
