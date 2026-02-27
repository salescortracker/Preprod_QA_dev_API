using System;
using System.Collections.Generic;

namespace DataAccessLayer.DBContext;

public partial class DocumentType
{
    public int Id { get; set; }

    public int CompanyId { get; set; }

    public int RegionId { get; set; }

    public string TypeName { get; set; } = null!;

    public string? Description { get; set; }

    public bool IsActive { get; set; }

    public bool IsDeleted { get; set; }

    public int CreatedBy { get; set; }

    public DateTime CreatedAt { get; set; }

    public int? ModifiedBy { get; set; }

    public DateTime? ModifiedAt { get; set; }

    public virtual ICollection<EmployeeDocument> EmployeeDocuments { get; set; } = new List<EmployeeDocument>();

    public virtual ICollection<EmployeeForm> EmployeeForms { get; set; } = new List<EmployeeForm>();

    public virtual ICollection<EmployeeLetter> EmployeeLetters { get; set; } = new List<EmployeeLetter>();
}
