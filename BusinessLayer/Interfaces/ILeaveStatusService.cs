using BusinessLayer.Common;
using BusinessLayer.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Interfaces
{public interface ILeaveStatusService
    {
        Task<ApiResponse<IEnumerable<LeaveStatusDto>>>
           GetAllLeaveStatusAsync(int companyId, int regionId);

        Task<ApiResponse<LeaveStatusDto?>>
            GetByIdAsync(int id);

        Task<ApiResponse<LeaveStatusDto>>
            CreateAsync(LeaveStatusDto dto);

        Task<ApiResponse<LeaveStatusDto>>
            UpdateAsync(LeaveStatusDto dto);

        Task<ApiResponse<bool>>
            DeleteAsync(int id);
    }
}
