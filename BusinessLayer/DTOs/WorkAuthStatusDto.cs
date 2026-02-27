using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.DTOs
{
    public class WorkAuthStatusDto
    {
        public int StatusId { get; set; }
        public int CompanyId { get; set; }
        public int RegionId { get; set; }
        public string StatusName { get; set; } = null!;
    }
}
