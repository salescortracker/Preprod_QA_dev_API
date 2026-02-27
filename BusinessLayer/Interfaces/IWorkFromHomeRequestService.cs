using BusinessLayer.DTOs;
using DataAccessLayer.DBContext;

namespace BusinessLayer.Interfaces
{
    public interface IWorkFromHomeRequestService
    {
        // 🔹 CREATE
        Task<WfhremoteRequest> CreateWorkFromHomeRequest(
            WfhRequestCreateDto dto);

        // 🔹 EMPLOYEE – MY REQUESTS
        Task<IEnumerable<WfhremoteRequest>> GetMyWorkFromHomeRequests(
            int employeeId,
            int companyId,
            int? regionId);

        // 🔹 MANAGER – PENDING APPROVAL LIST
        Task<IEnumerable<WfhremoteRequest>> GetPendingWorkFromHomeRequests(
            int companyId,
            int? regionId,
            int managerId);

        // 🔹 SINGLE APPROVE / REJECT
        Task<bool> UpdateWorkFromHomeRequest(
            UpdateWorkFromHomeRequestDto dto);

        // 🔥 BULK APPROVE / REJECT
        Task<int> BulkApproveRejectWorkFromHome(
            BulkApproveRejectWorkFromHomeDto dto);
    }
}
