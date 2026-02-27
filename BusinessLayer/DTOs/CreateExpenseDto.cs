using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.DTOs
{
    public class CreateExpenseDto
    {
        public IFormFile? Receipt { get; set; }
        public string ProjectName { get; set; }
        public string Location { get; set; }
        public string Country { get; set; }
        public int ExpenseCategoryId { get; set; }
        //public int? DepartmentId { get; set; }
        public int? DepartmentId { get; set; }   // ✅ IMPORTANT
        public string CurrencyCode { get; set; }
        public decimal Amount { get; set; }
        public DateTime ExpenseDate { get; set; }
        public string Reason { get; set; }
        public int UserId { get; set; }
        public int CompanyId { get; set; }
        public int RegionId { get; set; }
        public string? ReceiptPath { get; set; }

        public string? ExpenseCategoryName { get; set; }
        public string? Status { get; set; }

        public string? departmentName { get; set; }
    }
}
