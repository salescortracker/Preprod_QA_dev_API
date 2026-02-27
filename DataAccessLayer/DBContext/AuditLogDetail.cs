using System;
using System.Collections.Generic;

namespace DataAccessLayer.DBContext;

public partial class AuditLogDetail
{
    public long AuditLogDetailId { get; set; }

    public long AuditLogId { get; set; }

    public string ColumnName { get; set; } = null!;

    public string? OldValue { get; set; }

    public string? NewValue { get; set; }

    public virtual AuditLog AuditLog { get; set; } = null!;
}
