using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.DTOs
{
    public class PayrollTransactionDto
    {
        public int PayrollId { get; set; }
        public int EmployeeId { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public decimal GrossSalary { get; set; }
        public decimal TotalDeductions { get; set; }
        public decimal NetSalary { get; set; }
        public string Status { get; set; }

        public int UserId { get; set; }
        public string? CompanyId { get; set; }
        public string? RegionId { get; set; }
        public List<PayrollDetailDto> Details { get; set; }
    }
}
