using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.DTOs
{
    public class ExpenseListDto
    {
        public int ExpenseId { get; set; }
        public string ProjectName { get; set; }
        public string ExpenseCategoryName { get; set; }
        public string Country { get; set; }
        public decimal Amount { get; set; }
        public DateTime ExpenseDate { get; set; }
        public string Reason { get; set; }
        public string Status { get; set; }
        public string CurrencyCode { get; set; }
    }
}
