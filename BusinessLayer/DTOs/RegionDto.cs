using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.DTOs
{
    public class RegionDto
    {
        public int? RegionID { get; set; }
        public int? CompanyID { get; set; }
        public string RegionName { get; set; } = null!;
        public string Country { get; set; } = null!;
        public bool IsNew => RegionID == 0;
        public int? userId { get; set; }
        public bool? isActive { get; set; }
    }
}
