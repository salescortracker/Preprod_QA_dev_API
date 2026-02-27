using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.DTOs
{
    public class CompanyDto
    {
        public int CompanyId { get; set; }
        public string companyName { get; set; } = null!;
        public string? companyCode { get; set; }
        public string? industryType { get; set; }
        public string? headquarters { get; set; }       
        public bool? isActive { get; set; }
        public int? userId { get; set; }
    }
   
}
