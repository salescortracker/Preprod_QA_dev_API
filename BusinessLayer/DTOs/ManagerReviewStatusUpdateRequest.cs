using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.DTOs
{
    public class ManagerReviewStatusUpdateRequest
    {
        public List<int> ReviewIds { get; set; }
        public string Status { get; set; }
        public int ManagerRating { get; set; }
        public string ManagerComments { get; set; }
        public int ManagerId { get; set; }
        public int UserId { get; set; }
        public int RegionId { get; set; }
        public int CompanyId { get; set; }
    }
}
