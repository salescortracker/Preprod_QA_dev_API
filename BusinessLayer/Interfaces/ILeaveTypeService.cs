using BusinessLayer.Common;
using BusinessLayer.DTOs;

namespace BusinessLayer.Interfaces
{
    public interface ILeaveTypeService
    {
        Task<List<LeaveTypeDto>> GetLeaveTypesAsync();
        Task<bool> CreateLeaveTypeAsync(LeaveTypeDto dto);
        Task<bool> UpdateLeaveTypeAsync(LeaveTypeDto dto);
        Task<ApiResponse<IEnumerable<LeaveTypeDto>>> GetCRLeaveTypesAsync(
    int companyId,
    int regionId);
        Task<bool> DeleteLeaveTypeAsync(int id);
    }
}
