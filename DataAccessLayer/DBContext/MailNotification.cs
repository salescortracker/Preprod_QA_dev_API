using System;
using System.Collections.Generic;

namespace DataAccessLayer.DBContext;

public partial class MailNotification
{
    public int NotificationId { get; set; }

    public int UserId { get; set; }

    public string Title { get; set; } = null!;

    public string Message { get; set; } = null!;

    public bool? IsRead { get; set; }

    public DateTime? CreatedAt { get; set; }
}
