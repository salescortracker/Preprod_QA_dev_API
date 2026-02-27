using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.DTOs
{
    public class VisaTypeDto
    {
        public int VisaTypeId { get; set; }
        public int CompanyId { get; set; }
        public int RegionId { get; set; }
        public string VisaTypeName { get; set; } = null!;
    }
}
