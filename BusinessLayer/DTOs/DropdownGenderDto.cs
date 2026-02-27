using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.DTOs
{
    public class DropdownGenderDto
    {
        public int GenderId { get; set; }
        public string GenderName { get; set; } = string.Empty;
    }
}
