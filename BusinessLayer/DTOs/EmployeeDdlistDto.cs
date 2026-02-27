using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.DTOs
{
    public class EmployeeDdlistDto
    {
        public int DdlistId { get; set; }
        public int EmployeeId { get; set; }
        public int? RegionId { get; set; }
        public int? UserId { get; set; }
        public int? CompanyId { get; set; }
        public string Ddnumber { get; set; } = null!;
        public DateOnly Dddate { get; set; }
        public string BankName { get; set; } = null!;
        public string BranchName { get; set; } = null!;
        public decimal Amount { get; set; }
        public string PayeeName { get; set; } = null!;
        public string? DdcopyFilePath { get; set; }
    }
}
