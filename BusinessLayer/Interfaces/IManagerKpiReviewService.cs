using BusinessLayer.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Interfaces
{
    public interface IManagerKpiReviewService
    {
        Task<List<ManagerKpiReviewDto>> GetAllPendingReviewsAsync(int managerId);
        Task<bool> UpdateReviewAsync(ManagerReviewUpdateRequest request);
        Task<bool> UpdateStatusAsync(ManagerReviewStatusUpdateRequest request);
        Task<ManagerKpiReviewDto> GetReviewByIdAsync(int reviewId);

    }
}
