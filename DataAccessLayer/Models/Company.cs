using DataAccessLayer.DBContext;
using System;
using System.Collections.Generic;

namespace DataAccessLayer.Models;

public partial class Company
{
    public int CompanyId { get; set; }

    public string CompanyName { get; set; } = null!;

    public string? CompanyCode { get; set; }

    public string? IndustryType { get; set; }

    public string? Headquarters { get; set; }

    public bool? IsActive { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? CreatedDate { get; set; }

    public int? ModifiedBy { get; set; }

    public DateTime? ModifiedAt { get; set; }

    public virtual ICollection<Region> Regions { get; set; } = new List<Region>();

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
