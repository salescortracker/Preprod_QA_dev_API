using BusinessLayer.Common;
using BusinessLayer.DTOs;

namespace BusinessLayer.Interfaces
{
    public interface IScreeningResultService
    {
        Task<ApiResponse<IEnumerable<ScreeningResultDto>>> GetAll(int userId);

        Task<ApiResponse<ScreeningResultDto?>> GetByIdAsync(int id);

        Task<ApiResponse<string>> CreateAsync(ScreeningResultDto dto);

        Task<ApiResponse<string>> UpdateAsync(ScreeningResultDto dto);

        Task<ApiResponse<string>> DeleteAsync(int id);
    }
}
