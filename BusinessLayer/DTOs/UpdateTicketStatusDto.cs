using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.DTOs
{
    public class UpdateTicketStatusDto
    {
        public int TicketId { get; set; }
        public string Status { get; set; } = null!; // Approved / Rejected
        public string? ManagerComments { get; set; }
        public int ManagerId { get; set; }
    }
}
