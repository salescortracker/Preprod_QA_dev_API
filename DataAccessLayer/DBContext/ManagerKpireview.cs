using System;
using System.Collections.Generic;

namespace DataAccessLayer.DBContext;

public partial class ManagerKpireview
{
    public int ReviewId { get; set; }

    public int KpiitemId { get; set; }

    public int ManagerId { get; set; }

    public int? ManagerRating { get; set; }

    public decimal? AvgRating { get; set; }

    public string? ManagerComments { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? ModifiedAt { get; set; }

    public string Status { get; set; } = null!;

    public int UserId { get; set; }

    public int RegionId { get; set; }

    public int CompanyId { get; set; }
}
