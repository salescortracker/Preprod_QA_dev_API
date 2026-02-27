using System;
using System.Collections.Generic;

namespace DataAccessLayer.DBContext;

public partial class EmployeeBankDetail
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

    public int? AccountTypeId { get; set; }

    public string? Ifsccode { get; set; }

    public string? Micrcode { get; set; }

    public string? Upiid { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? ModifiedAt { get; set; }

    public int? CreatedBy { get; set; }

    public int? ModifiedBy { get; set; }

    public virtual AccountType? AccountType { get; set; }
}
