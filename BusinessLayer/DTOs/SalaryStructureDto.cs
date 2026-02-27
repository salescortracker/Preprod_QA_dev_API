using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.DTOs
{
    public class SalaryStructureDto
    {
        public int StructureId { get; set; }
        public string StructureName { get; set; }

        public int? DepartmentId { get; set; }
        public int? DesignationId { get; set; }
        public int? GradeId { get; set; }

        public bool IsActive { get; set; }

        public int? UserId { get; set; }
        public string? CompanyId { get; set; }
        public string? RegionId { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }

        public List<SalaryStructureComponentDto>? Components { get; set; }
    }
}
