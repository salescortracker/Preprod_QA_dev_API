using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.DTOs
{
    public class ExpenseApprovalListDto
    {
        public int ExpenseId { get; set; }
        public string ProjectName { get; set; }
        public string ExpenseCategoryName { get; set; }
        public string Country { get; set; }
        public decimal Amount { get; set; }
        public DateTime ExpenseDate { get; set; }
        public string UserFullName { get; set; }
        public string Status { get; set; }
        public int ExpenseCategoryId { get; set; }     // ✅ REQUIRED
    }
}
