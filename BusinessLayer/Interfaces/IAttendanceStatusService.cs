using BusinessLayer.Common;
using BusinessLayer.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Interfaces
{
    public interface IAttendanceStatusService
    {
        Task<ApiResponse<IEnumerable<AttendanceStatusDto>>>
           GetAllAsync(int companyId, int regionId);

        Task<ApiResponse<AttendanceStatusDto?>>
            GetByIdAsync(int id);

        Task<ApiResponse<AttendanceStatusDto>>
            CreateAsync(AttendanceStatusDto dto);

        Task<ApiResponse<AttendanceStatusDto>>
            UpdateAsync(AttendanceStatusDto dto);

        Task<ApiResponse<bool>>
            DeleteAsync(int id);
    }
}
