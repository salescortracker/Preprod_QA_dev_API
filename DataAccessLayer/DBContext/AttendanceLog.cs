using System;
using System.Collections.Generic;

namespace DataAccessLayer.DBContext;

public partial class AttendanceLog
{
    public int AttendanceId { get; set; }

    public int? BuildingId { get; set; }

    public DateTime? EntryTime { get; set; }

    public decimal? Latitude { get; set; }

    public decimal? Longitude { get; set; }

    public string? Status { get; set; }

    public int UserId { get; set; }

    public DateTime? ExitTime { get; set; }

    public virtual Building? Building { get; set; }
}
