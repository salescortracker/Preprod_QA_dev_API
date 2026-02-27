using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.DTOs
{
    public class HolidayListDto
    {
        public int HolidayListID { get; set; }
        public int CompanyID { get; set; }
        public int RegionID { get; set; }
        public string HolidayListName { get; set; } = string.Empty;
        public DateOnly? Date { get; set; }
        public bool IsActive { get; set; }
    }
}
