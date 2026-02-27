using System;
using System.Collections.Generic;

namespace DataAccessLayer.DBContext;

public partial class Building
{
    public int BuildingId { get; set; }

    public string? BuildingName { get; set; }

    public decimal? Latitude { get; set; }

    public decimal? Longitude { get; set; }

    public int? RadiusInMeters { get; set; }

    public bool? IsActive { get; set; }

    public DateTime? CreatedOn { get; set; }

    public virtual ICollection<AttendanceLog> AttendanceLogs { get; set; } = new List<AttendanceLog>();
}
