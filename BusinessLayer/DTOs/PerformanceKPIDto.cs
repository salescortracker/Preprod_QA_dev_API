using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.DTOs
{
    public class PerformanceKPIDto
    {
        public int? Id { get; set; }
        public string? KPIName { get; set; }
        public decimal? Weightage { get; set; }
        public string? Target { get; set; }
        public string? Achieved { get; set; }
        public int? SelfRating { get; set; }
        public int? ManagerRating { get; set; }
        public string? Remarks { get; set; }
    }
}
