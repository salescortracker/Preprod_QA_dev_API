using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.DTOs
{
    public class RecruiterDto
    {
        public int UserId { get; set; }
        public string FullName { get; set; } = null!;
    }
}
