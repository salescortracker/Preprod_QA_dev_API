using System;
using System.Collections.Generic;

namespace DataAccessLayer.DBContext;

public partial class AuditLog
{
    public long AuditLogId { get; set; }

    public long? UserId { get; set; }

    public string? UserName { get; set; }

    public string ActionType { get; set; } = null!;

    public string TableName { get; set; } = null!;

    public string? RecordId { get; set; }

    public string? Ipaddress { get; set; }

    public string? Device { get; set; }

    public DateTime? CreatedAt { get; set; }

    public string? Remarks { get; set; }

    public virtual ICollection<AuditLogDetail> AuditLogDetails { get; set; } = new List<AuditLogDetail>();
}
