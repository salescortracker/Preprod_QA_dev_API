using System;
using System.Collections.Generic;

namespace DataAccessLayer.DBContext;

public partial class EventType
{
    public int EventTypeId { get; set; }

    public int CompanyId { get; set; }

    public int RegionId { get; set; }

    public int UserId { get; set; }

    public string EventTypeName { get; set; } = null!;

    public bool IsActive { get; set; }

    public int CreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public int? ModifiedBy { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public virtual ICollection<Event> Events { get; set; } = new List<Event>();
}
