using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.DTOs
{
    public class ApproveRejectExpenseDto
    {
        public List<int> ExpenseIds { get; set; }
        public string Action { get; set; } // Approved / Rejected
        public int ManagerId { get; set; }
    }
}
