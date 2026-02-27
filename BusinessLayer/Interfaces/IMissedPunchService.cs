using BusinessLayer.DTOs;
using DataAccessLayer.DBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Interfaces
{
    public interface IMissedPunchService
    {
        Task<MissedPunchRequest> CreateMissedPunchRequest(CreateMissedPunchRequestDto dto);

        Task<IEnumerable<MissedPunchRequest>> GetMissedPunchRequest(
            int companyId, int? regionId);

        Task<IEnumerable<MissedPunchRequest>> GetApprovalMissedPunchRequest(
            int companyId, int? regionId, int managerId);

        Task<bool> UpdateMissedPunch(UpdateMissedPunchDto dto);

        Task<int> BulkApproveRejectPunch(BulkApproveRejectPunchDto dto);
    }
}
