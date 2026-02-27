using BusinessLayer.DTOs;
using BusinessLayer.Interfaces;
using DataAccessLayer.DBContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Implementations
{
    public class ManagerKpiReviewService:IManagerKpiReviewService
    {
        private readonly HRMSContext _context;

        public ManagerKpiReviewService(HRMSContext context)
        {
            _context = context;
        }

        // ===================== GET ALL PENDING REVIEWS ======================
        public async Task<List<ManagerKpiReviewDto>> GetAllPendingReviewsAsync(int managerId)
        {
            // Fetch KPI items
            var kpiItems = await (
                from item in _context.EmployeeKpiitems
                join kpi in _context.EmployeeKpis on item.Kpiid equals kpi.Kpiid
                where kpi.ReportingManagerId == managerId
                select new
                {
                    item.KpiitemId,
                    kpi.UserId
                }
            ).ToListAsync();

            // 🔥 Ensure entries exist
            foreach (var row in kpiItems)
            {
                bool exists = await _context.ManagerKpireviews
                    .AnyAsync(x => x.KpiitemId == row.KpiitemId);

                if (!exists)
                {
                    var user = await _context.Users
                        .FirstOrDefaultAsync(u => u.UserId == row.UserId);

                    var newReview = new ManagerKpireview
                    {
                        KpiitemId = row.KpiitemId,
                        UserId = row.UserId,
                        ManagerId = managerId,
                        RegionId = user?.RegionId ?? 0,
                        CompanyId = user?.CompanyId ?? 0,
                        Status = "Pending",
                        CreatedAt = DateTime.UtcNow
                    };

                    _context.ManagerKpireviews.Add(newReview);
                }
            }

            await _context.SaveChangesAsync();

            // Fetch final list
            var result = await (
                from item in _context.EmployeeKpiitems
                join kpi in _context.EmployeeKpis on item.Kpiid equals kpi.Kpiid
                join review in _context.ManagerKpireviews on item.KpiitemId equals review.KpiitemId
                join user in _context.Users on kpi.UserId equals user.UserId
                where kpi.ReportingManagerId == managerId
                select new ManagerKpiReviewDto
                {
                    ReviewId = review.ReviewId,
                    KPIItemId = item.KpiitemId,
                    EmployeeName = kpi.EmployeeNameId,
                    EmployeeId = kpi.UserId.ToString(),
                    AppraisalYear = kpi.AppraisalYear ?? 0,
                    KPIObjective = item.Kpiobjective ?? "",
                    Weightage = item.Weightage ?? 0,
                    Target = item.Target ?? "",
                    TaskCompleted = item.TaskCompleted ?? "",
                    SelfRating = item.SelfRating ?? 0,
                    ManagerRating = review.ManagerRating ?? 0,
                    AvgRating = review.AvgRating ?? 0m,
                    ManagerComments = review.ManagerComments ?? "",
                    Status = review.Status,
                    EmployeeEmail = user.Email,
                    UserId = kpi.UserId,
                    RegionId = review.RegionId,
                    CompanyId = review.CompanyId,
                    ManagerId = review.ManagerId
                }).ToListAsync();

            return result;
        }


        // ===================== GET SINGLE REVIEW ======================
        public async Task<ManagerKpiReviewDto?> GetReviewByIdAsync(int reviewId)
        {
            return await (
                from r in _context.ManagerKpireviews
                join item in _context.EmployeeKpiitems on r.KpiitemId equals item.KpiitemId
                join kpi in _context.EmployeeKpis on item.Kpiid equals kpi.Kpiid
                join user in _context.Users on r.UserId equals user.UserId
                where r.ReviewId == reviewId
                select new ManagerKpiReviewDto
                {
                    ReviewId = r.ReviewId,
                    KPIItemId = item.KpiitemId,
                    EmployeeName = kpi.EmployeeNameId,
                    EmployeeId = kpi.UserId.ToString(),
                    AppraisalYear = kpi.AppraisalYear ?? 0,
                    KPIObjective = item.Kpiobjective ?? "",
                    Weightage = item.Weightage ?? 0,
                    Target = item.Target ?? "",
                    TaskCompleted = item.TaskCompleted ?? "",
                    SelfRating = item.SelfRating ?? 0,
                    ManagerRating = r.ManagerRating ?? 0,
                    AvgRating = r.AvgRating ?? 0m,
                    ManagerComments = r.ManagerComments ?? "",
                    Status = r.Status,
                    EmployeeEmail = user.Email,
                    UserId = r.UserId,
                    RegionId = r.RegionId,
                    CompanyId = r.CompanyId,
                    ManagerId = r.ManagerId
                }).FirstOrDefaultAsync();
        }


        // ===================== UPDATE REVIEW ======================
        public async Task<bool> UpdateReviewAsync(ManagerReviewUpdateRequest request)
        {
            var review = await _context.ManagerKpireviews
                .FirstOrDefaultAsync(x => x.ReviewId == request.ReviewId);

            if (review == null) return false;

            review.ManagerRating = request.ManagerRating;
            review.AvgRating = request.ManagerRating;
            review.ManagerComments = request.ManagerComments;
            review.ModifiedAt = DateTime.UtcNow;

            review.ManagerId = request.ManagerId;
            review.UserId = request.UserId;
            review.RegionId = request.RegionId;
            review.CompanyId = request.CompanyId;

            await _context.SaveChangesAsync();
            return true;
        }


        // ===================== UPDATE STATUS ======================
        public async Task<bool> UpdateStatusAsync(ManagerReviewStatusUpdateRequest request)
        {
            var reviews = await _context.ManagerKpireviews
                .Where(x => request.ReviewIds.Contains(x.ReviewId))
                .ToListAsync();

            foreach (var r in reviews)
            {
                r.Status = request.Status;
                r.ManagerRating = request.ManagerRating;
                r.AvgRating = request.ManagerRating;  // optional logic
                r.ManagerComments = request.ManagerComments;
                r.ModifiedAt = DateTime.UtcNow;

                r.ManagerId = request.ManagerId;
                r.UserId = request.UserId;
                r.RegionId = request.RegionId;
                r.CompanyId = request.CompanyId;
            }

            await _context.SaveChangesAsync();
            return true;
        }
    }
}
