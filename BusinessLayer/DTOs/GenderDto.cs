using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.DTOs
{
    public class GenderDto
    {
        public int genderID { get; set; }
        public string genderName { get; set; } = string.Empty;
        public bool IsActive { get; set; }

        // New Fields
        public int companyID { get; set; }
        public int regionId { get; set; }
        public int? userId { get; set; }
        public string companyName { get; set; }
        public string regionName { get; set; }
    }
}
