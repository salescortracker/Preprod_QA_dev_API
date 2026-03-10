using BusinessLayer.Common;
using BusinessLayer.DTOs;

namespace BusinessLayer.Interfaces
{
    public interface IInterviewLevelService
    {
        Task<ApiResponse<IEnumerable<InterviewLevelDto>>> GetAll(int userId);

        Task<ApiResponse<InterviewLevelDto?>> GetByIdAsync(int id);

        Task<ApiResponse<string>> CreateAsync(InterviewLevelDto dto);

        Task<ApiResponse<string>> UpdateAsync(InterviewLevelDto dto);

        Task<ApiResponse<string>> DeleteAsync(int id);
    }
}
