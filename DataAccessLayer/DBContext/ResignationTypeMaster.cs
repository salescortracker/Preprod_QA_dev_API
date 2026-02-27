using System;
using System.Collections.Generic;

namespace DataAccessLayer.DBContext;

public partial class ResignationTypeMaster
{
    public int ResignationTypeId { get; set; }

    public string? TypeName { get; set; }

    public bool? IsActive { get; set; }
}
