using System;
using System.Collections.Generic;

namespace DataAccessLayer.DBContext;

public partial class Event
{
    public int EventId { get; set; }

    public int CompanyId { get; set; }

    public int RegionId { get; set; }

    public int UserId { get; set; }

    public int EventTypeId { get; set; }

    public string EventName { get; set; } = null!;

    public DateOnly EventDate { get; set; }

    public string? Description { get; set; }

    public bool IsActive { get; set; }

    public int CreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public int? ModifiedBy { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public int? RoleId { get; set; }

    public virtual EventType EventType { get; set; } = null!;
}
