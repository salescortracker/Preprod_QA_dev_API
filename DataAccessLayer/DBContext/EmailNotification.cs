using System;
using System.Collections.Generic;

namespace DataAccessLayer.DBContext;

public partial class EmailNotification
{
    public int EmailNotificationId { get; set; }

    public string ModuleName { get; set; } = null!;

    public string EventType { get; set; } = null!;

    public int ActionByUserId { get; set; }

    public string ToEmail { get; set; } = null!;

    public string? EmailSubject { get; set; }

    public string? EmailBody { get; set; }

    public string RouteUrl { get; set; } = null!;

    public string EmailStatus { get; set; } = null!;

    public int RetryCount { get; set; }

    public string? ErrorMessage { get; set; }

    public DateTime CreatedDate { get; set; }

    public DateTime? SentDate { get; set; }
}
