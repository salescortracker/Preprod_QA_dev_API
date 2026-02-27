using System;
using System.Collections.Generic;

namespace DataAccessLayer.DBContext;

public partial class EmployeeDocument
{
    public int Id { get; set; }

    public int RegionId { get; set; }

    public int CompanyId { get; set; }

    public int UserId { get; set; }

    public int DocumentTypeId { get; set; }

    public string? DocumentName { get; set; }

    public string DocumentNumber { get; set; } = null!;

    public DateOnly IssuedDate { get; set; }

    public DateOnly ExpiryDate { get; set; }

    public string FileName { get; set; } = null!;

    public string? FilePath { get; set; }

    public string? Remarks { get; set; }

    public bool IsConfidential { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime CreatedAt { get; set; }

    public int? ModifiedBy { get; set; }

    public DateTime? ModifiedAt { get; set; }

    public virtual DocumentType DocumentType { get; set; } = null!;
}
