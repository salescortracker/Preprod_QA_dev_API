using BusinessLayer.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Interfaces
{
    public interface ILeaveService
    {
        Task<IEnumerable<LeaveTypeDto>> GetActiveLeaveTypesAsync();
        Task<ReportingManagerDto> GetReportingManagerAsync(int userId);
        Task<int> SubmitLeaveAsync(LeaveRequestDto dto);
        Task<IEnumerable<LeaveRequestDto>> GetMyLeavesAsync(int userId);
        Task SendLeaveEmailToManagerAsync(int leaveRequestId);
        Task<bool> ApproveLeaveFromEmailAsync(int leaveId);
        Task<bool> RejectLeaveFromEmailAsync(int leaveId);
        Task<IEnumerable<LeaveRequestDto>> GetLeavesForManagerAsync(int managerId);
        Task<bool> ApproveLeaveByManagerAsync(int leaveId);
        Task<bool> RejectLeaveByManagerAsync(int leaveId);
        Task<bool> BulkApproveLeavesAsync(List<int> leaveIds);
        Task<bool> BulkRejectLeavesAsync(List<int> leaveIds);
        Task<IEnumerable<LeaveRequestDto>> GetLeavesForUserAsync(int userId);
        Task<IEnumerable<LeaveRequestDto>> GetLeavesForManagerUserAsync(int managerId);
    }
}
