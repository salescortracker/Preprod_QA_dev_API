using System;
using System.Collections.Generic;

namespace DataAccessLayer.DBContext;

public partial class TaxSetting
{
    public int TaxId { get; set; }

    public string TaxName { get; set; } = null!;

    public int TaxTypeId { get; set; }

    public decimal Rate { get; set; }

    public DateOnly EffectiveDate { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual TaxType TaxType { get; set; } = null!;
}
