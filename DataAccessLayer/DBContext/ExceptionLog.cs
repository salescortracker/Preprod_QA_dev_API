using System;
using System.Collections.Generic;

namespace DataAccessLayer.DBContext;

public partial class ExceptionLog
{
    public long Id { get; set; }

    public string? ErrorCode { get; set; }

    public string ErrorMessage { get; set; } = null!;

    public string? StackTrace { get; set; }

    public string? InnerException { get; set; }

    public string? ErrorType { get; set; }

    public string? ControllerName { get; set; }

    public string? ActionName { get; set; }

    public string? UserId { get; set; }

    public string? RequestPath { get; set; }

    public string? RequestPayload { get; set; }

    public string? HostName { get; set; }

    public string? ClientIp { get; set; }

    public string? BrowserInfo { get; set; }

    public DateTime? CreatedDate { get; set; }
}
