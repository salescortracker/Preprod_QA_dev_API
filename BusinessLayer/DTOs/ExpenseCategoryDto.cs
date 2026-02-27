using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.DTOs
{
    public class ExpenseCategoryDto
    {
        public int ExpenseCategoryID { get; set; }
        public string ExpenseCategoryName { get; set; }
        public bool IsActive { get; set; }
        public int? SortOrder { get; set; }
        public string? Description { get; set; }
        public int CompanyId { get; set; }
        public int RegionId { get; set; }
    }
}
