using BusinessLayer.DTOs;
using BusinessLayer.Interfaces;
using DataAccessLayer.DBContext;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HRMS_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeKpiController : ControllerBase
    {
        private readonly IEmployeeKpiService _kpiService;
        private readonly IWebHostEnvironment _env;
        private readonly IPerformanceService _Iperformanceservice;
        private readonly IManagerKpiReviewService _managerReviewService;
        private readonly IEmailService _emailService;
        private readonly HRMSContext _context;

        public EmployeeKpiController(IEmployeeKpiService kpiService, IWebHostEnvironment env, IManagerKpiReviewService managerReviewService, HRMSContext context,
            IEmailService emailService, IPerformanceService Iperformanceservice)
        {
            _kpiService = kpiService;
            _env = env;
            _managerReviewService = managerReviewService;
            _emailService = emailService;
            _context = context;
            _Iperformanceservice = Iperformanceservice;
        }

        #region KPI Employee Operations
        /// <summary>
        /// Retrieves all employee KPIs.
        /// </summary>
        /// <returns>List of all KPIs.</returns>
        [HttpGet("GetAllKpis")]
        public async Task<IActionResult> GetAllKpis()
        {
            var kpis = await _kpiService.GetAllKpisAsync();
            return Ok(kpis);
        }

        /// <summary>
        /// Retrieves all KPIs for a specific user.
        /// </summary>
        /// <param name="userId">ID of the user.</param>
        /// <returns>List of KPIs for the given user.</returns>
        [HttpGet("GetKpisByUser/{userId}")]
        public async Task<IActionResult> GetKpisByUser(int userId)
        {
            var kpis = await _kpiService.GetKpisByUserAsync(userId);
            return Ok(kpis);
        }

        /// <summary>
        /// Creates a new KPI entry for an employee.
        /// </summary>
        /// <param name="dto">EmployeeKpiDto containing KPI data and optional file upload.</param>
        /// <returns>ID of the created KPI and success message.</returns>
        [HttpPost("CreateKpi")]
        public async Task<IActionResult> CreateKpi([FromForm] EmployeeKpiDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Required fields check
            if (dto.UserId <= 0 || dto.CompanyId <= 0 || dto.RegionId <= 0)
                return BadRequest("UserId, CompanyId, and RegionId are required.");

            // FILE UPLOAD
            if (Request.Form.Files.Count > 0)
            {
                var file = Request.Form.Files[0];
                var root = _env.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
                var uploadFolder = Path.Combine(root, "Uploads", "EmployeeKpiDocuments");

                if (!Directory.Exists(uploadFolder))
                    Directory.CreateDirectory(uploadFolder);

                var fileName = $"{Guid.NewGuid()}_{file.FileName}";
                var fullPath = Path.Combine(uploadFolder, fileName);

                using var stream = new FileStream(fullPath, FileMode.Create);
                await file.CopyToAsync(stream);

                dto.DocumentEvidencePath = Path.Combine("Uploads", "EmployeeKpiDocuments", fileName)
                                                        .Replace("\\", "/");
            }

            // SAVE KPI
            dto.CreatedBy = dto.UserId;
            dto.CreatedAt = DateTime.Now;

            var kpiId = await _kpiService.CreateKpiAsync(dto);

            // =================================================
            //      🚀 SEND EMAIL TO REPORTING MANAGER
            // =================================================

            // 1. Fetch manager email from Users table
            var manager = await _context.Users
                .Where(u => u.UserId == dto.ReportingManagerId)
                .FirstOrDefaultAsync();

            string? managerEmail = manager?.Email;

            if (string.IsNullOrEmpty(managerEmail))
            {
                return Ok(new
                {
                    Message = "KPI created but manager email not found.",
                    KpiId = kpiId
                });
            }

            // 2. Prepare email body
            string subject = $"New KPI Submitted by {dto.EmployeeNameId}";
            string body = $@"
        <h2>KPI Submitted</h2>
        <p><b>Employee:</b> {dto.EmployeeNameId}</p>
        <p><b>Appraisal Year:</b> {dto.AppraisalYear}</p>
        <p><b>Reporting Manager ID:</b> {dto.ReportingManagerId}</p>
        <p>Please log in to HRMS Manager Dashboard to review the submission.</p>
    ";

            try
            {
                await _emailService.SendEmailAsync(managerEmail, subject, body);
            }
            catch (Exception ex)
            {
                return Ok(new
                {
                    Message = "KPI created but failed to send email to manager.",
                    Error = ex.Message,
                    KpiId = kpiId
                });
            }

            return Ok(new { Message = "KPI created successfully & Email sent to Reporting Manager", KpiId = kpiId });
        }

        /// <summary>
        /// Updates an existing KPI entry for an employee.
        /// </summary>
        /// <param name="dto">EmployeeKpiDto containing updated KPI data and optional file upload.</param>
        /// <returns>Success or not found message.</returns>
        [HttpPost("UpdateKpi")]
        public async Task<IActionResult> UpdateKpi([FromForm] EmployeeKpiDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Handle file upload
            if (Request.Form.Files.Count > 0)
            {
                var file = Request.Form.Files[0];
                string root = _env.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
                string uploadFolder = Path.Combine(root, "Uploads", "EmployeeKpiDocuments");

                if (!Directory.Exists(uploadFolder))
                    Directory.CreateDirectory(uploadFolder);

                string fileName = $"{Guid.NewGuid()}_{file.FileName}";
                string fullPath = Path.Combine(uploadFolder, fileName);

                using var stream = new FileStream(fullPath, FileMode.Create);
                await file.CopyToAsync(stream);

                // Save relative path
                dto.DocumentEvidencePath = Path.Combine("Uploads", "EmployeeKpiDocuments", fileName)
                    .Replace("\\", "/");
            }

            // Set auditing
            dto.ModifiedBy = dto.UserId;
            dto.ModifiedAt = DateTime.Now;

            var isUpdated = await _kpiService.UpdateKpiAsync(dto);
            if (!isUpdated)
                return NotFound(new { Message = "KPI not found" });

            return Ok(new { Message = "KPI updated successfully" });
        }

        /// <summary>
        /// Deletes a KPI entry by its ID.
        /// </summary>
        /// <param name="kpiId">ID of the KPI to delete.</param>
        /// <returns>No content if deleted, or not found message.</returns>
        [HttpDelete("DeleteKpi/{kpiId}")]
        public async Task<IActionResult> DeleteKpi(int kpiId)
        {
            var isDeleted = await _kpiService.DeleteKpiAsync(kpiId);

            if (!isDeleted)
                return NotFound(new { Message = "KPI not found" });

            return NoContent();
        }

        #endregion


        #region Manager KPI Review Operations

        // ===================== MANAGER KPI REVIEW =====================

        [HttpGet("Manager/GetReviews/{managerId}")]
        public async Task<IActionResult> GetManagerReviews(int managerId)
        {
            var reviews = await _managerReviewService.GetAllPendingReviewsAsync(managerId);
            return Ok(reviews);
        }

        [HttpPut("Manager/UpdateReview")]
        public async Task<IActionResult> UpdateManagerReview([FromBody] ManagerReviewUpdateRequest request)
        {
            var updated = await _managerReviewService.UpdateReviewAsync(request);
            if (!updated)
                return NotFound(new { Message = "Review not found" });

            return Ok(new { Message = "Review updated successfully" });
        }

        [HttpPut("Manager/UpdateStatus")]
        public async Task<IActionResult> UpdateManagerReviewStatus([FromBody] ManagerReviewStatusUpdateRequest request)
        {
            var updated = await _managerReviewService.UpdateStatusAsync(request);
            if (!updated)
                return BadRequest(new { Message = "Failed to update status" });

            foreach (var reviewId in request.ReviewIds)
            {
                var review = await _managerReviewService.GetReviewByIdAsync(reviewId);

                if (review != null && !string.IsNullOrEmpty(review.EmployeeEmail))
                {
                    string subject = $"KPI Review {request.Status}";
                    string body = $"Dear {review.EmployeeName},<br/><br/>" +
                                  $"Your KPI for {review.AppraisalYear} is <b>{request.Status}</b>.<br/>" +
                                  $"Comments: {review.ManagerComments}<br/><br/>" +
                                  "Regards,<br/>HRMS Team";

                    await _emailService.SendEmailAsync(review.EmployeeEmail, subject, body);
                }
            }

            return Ok(new { Message = $"Status updated to {request.Status}" });
        }

        // 1. Save Draft
        [HttpPost("SaveDraft")]
        public async Task<IActionResult> SaveDraft([FromBody] PerformanceReviewDto dto)
        {
            dto.Status = "Draft";
            var result = await _Iperformanceservice.SaveAsync(dto);
            return Ok(result);
        }

        // 2. Submit
        [HttpPost("Submit")]
        public async Task<IActionResult> Submit([FromBody] PerformanceReviewDto dto)
        {
            dto.Status = "Submitted";
            var result = await _Iperformanceservice.SaveAsync(dto);
            return Ok(result);
        }

        // 3. Get Manager Reviews
        [HttpGet("managerReview/{managerId}")]
        public async Task<IActionResult> GetManagerReviewsData(int managerId)
        {
            var result = await _Iperformanceservice.GetManagerReviewsAsync(managerId);
            return Ok(result);
        }

        // 4. Approve
        [HttpPost("Approve")]
        public async Task<IActionResult> Approve(int reviewId, int managerId, string remarks)
        {
            var result = await _Iperformanceservice.ApproveAsync(reviewId, managerId, remarks);
            return Ok(result);
        }

        // 5. Reject
        [HttpPost("Reject")]
        public async Task<IActionResult> Reject(int reviewId, int managerId, string remarks)
        {
            var result = await _Iperformanceservice.RejectAsync(reviewId, managerId, remarks);
            return Ok(result);
        }
        #endregion


    }
}
