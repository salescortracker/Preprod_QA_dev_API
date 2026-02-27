using BusinessLayer.Common;
using BusinessLayer.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Interfaces
{
    public interface IPriorityService
    {
        Task<ApiResponse<IEnumerable<PriorityDto>>> GetAll(int userId);

        Task<ApiResponse<PriorityDto?>> GetByIdAsync(int id);

        Task<ApiResponse<string>> CreateAsync(PriorityDto dto);

        Task<ApiResponse<string>> UpdateAsync(PriorityDto dto);

        Task<ApiResponse<string>> DeleteAsync(int id);
    }
}
