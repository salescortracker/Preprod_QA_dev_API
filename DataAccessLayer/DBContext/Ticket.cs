using System;
using System.Collections.Generic;

namespace DataAccessLayer.DBContext;

public partial class Ticket
{
    public int TicketId { get; set; }

    public int RegionId { get; set; }

    public int CompanyId { get; set; }

    public int UserId { get; set; }

    public string TicketNumber { get; set; } = null!;

    public int CategoryId { get; set; }

    public string Subject { get; set; } = null!;

    public int PriorityId { get; set; }

    public string Description { get; set; } = null!;

    public string FileName { get; set; } = null!;

    public string? FilePath { get; set; }

    public string? Status { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime CreatedAt { get; set; }

    public int? ModifiedBy { get; set; }

    public DateTime? ModifiedAt { get; set; }

    public int? ManagerId { get; set; }

    public string? ManagerComments { get; set; }

    public DateTime? ApprovedAt { get; set; }

    public int? ApprovedBy { get; set; }

    public virtual HelpDeskCategory Category { get; set; } = null!;

    public virtual Priority Priority { get; set; } = null!;
}
