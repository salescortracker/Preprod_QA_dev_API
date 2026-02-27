using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.DTOs
{
    public class FilingStatusDto
    {
        public int Id { get; set; }
        public string StatusName { get; set; } = null!;
        public bool? IsActive { get; set; }
    }
}
