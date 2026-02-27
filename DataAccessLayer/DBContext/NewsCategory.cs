using System;
using System.Collections.Generic;

namespace DataAccessLayer.DBContext;

public partial class NewsCategory
{
    public int CategoryId { get; set; }

    public string CategoryName { get; set; } = null!;

    public string? Description { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public string? CreatedBy { get; set; }
}
