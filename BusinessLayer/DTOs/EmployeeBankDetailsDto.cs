using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.DTOs
{
    public class EmployeeBankDetailsDto
    {
        public int BankDetailsId { get; set; }
        public int EmployeeId { get; set; }
        public int? RegionId { get; set; }
        public int? UserId { get; set; }
        public int? CompanyId { get; set; }
        public string BankName { get; set; } = null!;
        public string BranchName { get; set; } = null!;
        public string AccountHolderName { get; set; } = null!;
        public string AccountNumber { get; set; } = null!;
        public int AccountTypeId { get; set; }
        public string? Ifsccode { get; set; }
        public string? Micrcode { get; set; }
        public string? Upiid { get; set; }
    }
}
