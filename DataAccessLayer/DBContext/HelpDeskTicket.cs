using System;
using System.Collections.Generic;

namespace DataAccessLayer.DBContext;

public partial class HelpDeskTicket
{
    public int TicketId { get; set; }

    public string TicketCode { get; set; } = null!;

    public int EmployeeId { get; set; }

    public string EmployeeName { get; set; } = null!;

    public int DepartmentId { get; set; }

    public string DepartmentName { get; set; } = null!;

    public string Category { get; set; } = null!;

    public string Subject { get; set; } = null!;

    public string Description { get; set; } = null!;

    public string Priority { get; set; } = null!;

    public string Status { get; set; } = null!;

    public DateTime RaisedOn { get; set; }

    public DateTime? LastUpdatedOn { get; set; }

    public int CreatedBy { get; set; }

    public DateTime CreatedAt { get; set; }

    public int? ModifiedBy { get; set; }

    public DateTime? ModifiedAt { get; set; }

    public bool? IsActive { get; set; }

    public bool? IsDeleted { get; set; }
}
