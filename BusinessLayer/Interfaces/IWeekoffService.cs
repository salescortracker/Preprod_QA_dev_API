using BusinessLayer.Common;
using BusinessLayer.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Interfaces
{
    public interface IWeekoffService
    {
        Task<ApiResponse<IEnumerable<WeekoffDto>>> GetAll(int userId);
        Task<ApiResponse<WeekoffDto?>> GetByIdAsync(int id);
        Task<ApiResponse<string>> CreateAsync(WeekoffDto dto);
        Task<ApiResponse<string>> UpdateAsync(WeekoffDto dto);
        Task<ApiResponse<string>> DeleteAsync(int id);
    }
}
