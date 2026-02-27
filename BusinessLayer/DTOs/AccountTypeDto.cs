using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.DTOs
{
    public class AccountTypeDto
    {
        public int AccountTypeId { get; set; }
        public string AccountTypeName { get; set; } = null!;
        public bool? IsActive { get; set; }
    }
}
