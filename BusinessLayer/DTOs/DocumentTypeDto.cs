using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.DTOs
{
    public class DocumentTypeDto
    {
        public int Id { get; set; }
        public string TypeName { get; set; } = null!;
        public bool? IsActive { get; set; }
    }
}
