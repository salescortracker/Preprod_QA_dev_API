using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLayer.Common;
using BusinessLayer.DTOs;

namespace BusinessLayer.Interfaces
{
    public interface IRecruitmentNoticePeriodService
    {
        Task<ApiResponse<IEnumerable<RecruitmentNoticePeriodDto>>> GetAllRecruitmentNoticePeriodService(int userId);
        Task<ApiResponse<RecruitmentNoticePeriodDto?>> GetByIdRecruitmentNoticePeriodServiceAsync(int id);
        Task<ApiResponse<string>> CreateRecruitmentNoticePeriodServiceAsync(RecruitmentNoticePeriodDto dto);
        Task<ApiResponse<string>> UpdateRecruitmentNoticePeriodServiceAsync(RecruitmentNoticePeriodDto dto);
        Task<ApiResponse<string>> DeleteRecruitmentNoticePeriodServiceAsync(int id);
    }
}
