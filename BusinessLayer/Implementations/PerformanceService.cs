using BusinessLayer.Common;
using BusinessLayer.DTOs;
using BusinessLayer.Interfaces;
using DataAccessLayer.DBContext;
using Microsoft.EntityFrameworkCore;

namespace BusinessLayer.Implementations
{
    public class PerformanceService: IPerformanceService
    {
        private readonly HRMSContext _context;

        public PerformanceService(HRMSContext context)
        {
            _context = context;
        }

        // ===============================
        // GET BY USER ID (Employee View)
        // ===============================
        public async Task<ApiResponse<IEnumerable<PerformanceReviewDto>>> GetByUserIdAsync(int userId)
        {
            var list = await _context.PerformanceReviews
                .Where(x => x.UserId == userId)
                .Include(x => x.KPIs)
                .OrderByDescending(x => x.Id)
                .ToListAsync();

            var result = list.Select(review => new PerformanceReviewDto
            {
                Id = review.Id,
                UserId = review.UserId,
                RoleId = review.RoleId,
                DepartmentProject = review.DepartmentProject,
                ReportingManagerId = review.ReportingManagerId,
                Designation = review.Designation,
                Department = review.Department,
                DateOfJoining = review.DateOfJoining,
                ProbationStatus = review.ProbationStatus,
                PerformanceCycle = review.PerformanceCycle,
                ApplicableStartDate = review.ApplicableStartDate,
                ApplicableEndDate = review.ApplicableEndDate,
                ProgressType = review.ProgressType,
                AppraisalYear = review.AppraisalYear,
                DocumentEvidence = review.DocumentEvidence,
                SelfReviewSummary = review.SelfReviewSummary,
                Status = review.Status,

                KPIs = review.KPIs?.Select(k => new PerformanceKPIDto
                {
                    Id = k.Id,
                    KPIName = k.Kpiname,
                    Weightage = k.Weightage,
                    Target = k.Target,
                    Achieved = k.Achieved,
                    SelfRating = k.SelfRating,
                    ManagerRating = k.ManagerRating,
                    Remarks = k.Remarks
                }).ToList()
            }).ToList();

            return new ApiResponse<IEnumerable<PerformanceReviewDto>>(result);
        }

        // ===============================
        // SAVE DRAFT / SUBMIT
        // ===============================
        public async Task<ApiResponse<bool>> SaveAsync(PerformanceReviewDto dto)
        {
            PerformanceReview review;

            if (dto.Id == null || dto.Id == 0)
            {
                review = new PerformanceReview
                {
                    UserId = dto.UserId,
                    RoleId = dto.RoleId,
                    DepartmentProject = dto.DepartmentProject,
                    ReportingManagerId = dto.ReportingManagerId,
                    Designation = dto.Designation,
                    Department = dto.Department,
                    DateOfJoining = dto.DateOfJoining,
                    ProbationStatus = dto.ProbationStatus,
                    PerformanceCycle = dto.PerformanceCycle,
                    ApplicableStartDate = dto.ApplicableStartDate,
                    ApplicableEndDate = dto.ApplicableEndDate,
                    ProgressType = dto.ProgressType,
                    AppraisalYear = dto.AppraisalYear,
                    DocumentEvidence = dto.DocumentEvidence,
                    SelfReviewSummary = dto.SelfReviewSummary,
                    Status = dto.Status,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = dto.UserId
                };

                _context.PerformanceReviews.Add(review);
                await _context.SaveChangesAsync();
            }
            else
            {
                review = await _context.PerformanceReviews
                    .Include(x => x.KPIs)
                    .FirstOrDefaultAsync(x => x.Id == dto.Id);

                if (review == null)
                    return new ApiResponse<bool>(false, "Record not found");

                review.DepartmentProject = dto.DepartmentProject;
                review.ProbationStatus = dto.ProbationStatus;
                review.PerformanceCycle = dto.PerformanceCycle;
                review.ApplicableStartDate = dto.ApplicableStartDate;
                review.ApplicableEndDate = dto.ApplicableEndDate;
                review.ProgressType = dto.ProgressType;
                review.AppraisalYear = dto.AppraisalYear;
                review.DocumentEvidence = dto.DocumentEvidence;
                review.SelfReviewSummary = dto.SelfReviewSummary;
                review.Status = dto.Status;
                review.ModifiedAt = DateTime.UtcNow;
                review.ModifiedBy = dto.UserId;

                await _context.SaveChangesAsync();

                // Remove old KPIs
                _context.PerformanceKpis.RemoveRange(review.KPIs);
                await _context.SaveChangesAsync();
            }

            // Save KPI List
            if (dto.KPIs != null && dto.KPIs.Any())
            {
                foreach (var kpi in dto.KPIs)
                {
                    var entity = new PerformanceKpi
                    {
                        PerformanceReviewId = review.Id,
                        Kpiname = kpi.KPIName,
                        Weightage = kpi.Weightage,
                        Target = kpi.Target,
                        Achieved = kpi.Achieved,
                        SelfRating = kpi.SelfRating,
                        ManagerRating = kpi.ManagerRating,
                        Remarks = kpi.Remarks
                    };

                    _context.PerformanceKpis.Add(entity);
                }

                await _context.SaveChangesAsync();
            }

            return new ApiResponse<bool>(true);
        }

        // ===============================
        // GET MANAGER REVIEWS
        // ===============================
        public async Task<ApiResponse<IEnumerable<PerformanceReviewDto>>> GetManagerReviewsAsync(int loggedInUserId)
        {
            var reviews = await (
                from review in _context.PerformanceReviews
                join user in _context.Users
                    on review.UserId equals user.UserId
                where review.ReportingManagerId == loggedInUserId
                select new { review, user }
            ).ToListAsync();

            if (!reviews.Any())
            {
                return new ApiResponse<IEnumerable<PerformanceReviewDto>>(
                    new List<PerformanceReviewDto>(),
                    "No records found"
                );
            }

            var result = reviews.Select(x => new PerformanceReviewDto
            {
                Id = x.review.Id,
                UserId = x.review.UserId,
                RoleId = x.review.RoleId,

                DepartmentProject = x.review.DepartmentProject,
                ReportingManagerId = x.review.ReportingManagerId,
                Designation = x.review.Designation,
                Department = x.review.Department,
                DateOfJoining = x.review.DateOfJoining,

                ProbationStatus = x.review.ProbationStatus,
                PerformanceCycle = x.review.PerformanceCycle,
                ApplicableStartDate = x.review.ApplicableStartDate,
                ApplicableEndDate = x.review.ApplicableEndDate,
                ProgressType = x.review.ProgressType,
                AppraisalYear = x.review.AppraisalYear,
                DocumentEvidence = x.review.DocumentEvidence,

                SelfReviewSummary = x.review.SelfReviewSummary,
                Status = x.review.Status,

                // 🔥 CORRECT WAY TO GET EMPLOYEE NAME
                EmployeeName = x.user.FullName,

                KPIs = _context.PerformanceKpis
                    .Where(k => k.PerformanceReviewId == x.review.Id)
                    .Select(k => new PerformanceKPIDto
                    {
                        Id = k.Id,
                        KPIName = k.Kpiname,
                        Weightage = k.Weightage,
                        Target = k.Target,
                        Achieved = k.Achieved,
                        SelfRating = k.SelfRating,
                        ManagerRating = k.ManagerRating,
                        Remarks = k.Remarks
                    }).ToList()
            }).ToList();

            return new ApiResponse<IEnumerable<PerformanceReviewDto>>(result);
        }

        // ===============================
        // APPROVE
        // ===============================
        public async Task<ApiResponse<bool>> ApproveAsync(int reviewId, int managerId, string remarks)
        {
            var review = await _context.PerformanceReviews
                .FirstOrDefaultAsync(x => x.Id == reviewId && x.ReportingManagerId == managerId);

            if (review == null)
                return new ApiResponse<bool>(false, "Record not found");

            review.Status = "Approved";
            review.ManagerRemarks = remarks;
            review.ModifiedAt = DateTime.UtcNow;
            review.ModifiedBy = managerId;

            await _context.SaveChangesAsync();

            return new ApiResponse<bool>(true);
        }

        // ===============================
        // REJECT
        // ===============================
        public async Task<ApiResponse<bool>> RejectAsync(int reviewId, int managerId, string remarks)
        {
            var review = await _context.PerformanceReviews
                .FirstOrDefaultAsync(x => x.Id == reviewId && x.ReportingManagerId == managerId);

            if (review == null)
                return new ApiResponse<bool>(false, "Record not found");

            review.Status = "Rejected";
            review.ManagerRemarks = remarks;
            review.ModifiedAt = DateTime.UtcNow;
            review.ModifiedBy = managerId;

            await _context.SaveChangesAsync();

            return new ApiResponse<bool>(true);
        }

        public async Task<ApiResponse<bool>> RequestAsync(int reviewId)
        {
            var review = await _context.PerformanceReviews
                .FirstOrDefaultAsync(x => x.Id == reviewId);

            if (review == null)
                return new ApiResponse<bool>(false, "Record not found");

            review.Status = "Submitted"; // reset to submitted
            review.ModifiedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return new ApiResponse<bool>(true);
        }


    }
}
