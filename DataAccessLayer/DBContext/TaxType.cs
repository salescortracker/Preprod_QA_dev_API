using System;
using System.Collections.Generic;

namespace DataAccessLayer.DBContext;

public partial class TaxType
{
    public int TaxTypeId { get; set; }

    public string TaxTypeName { get; set; } = null!;

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual ICollection<TaxSetting> TaxSettings { get; set; } = new List<TaxSetting>();
}
