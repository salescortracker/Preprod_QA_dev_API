using BusinessLayer.DTOs;
using BusinessLayer.Interfaces;
using DataAccessLayer.DBContext;
using DataAccessLayer.Repositories.GeneralRepository;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Implementations
{
    public class LeaveService:ILeaveService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _configuration;
        public LeaveService(IUnitOfWork unitOfWork, IEmailService emailService,
                            IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _emailService = emailService;
            _configuration = configuration;

        }

        public async Task<IEnumerable<LeaveTypeDto>> GetActiveLeaveTypesAsync()
        {
            var data = await _unitOfWork.Repository<LeaveType>().GetAllAsync();

            return data
                .Where(x => x.IsActive == true && x.IsDeleted == false)
                .Select(x => new LeaveTypeDto
                {
                    LeaveTypeID = x.LeaveTypeId,
                    LeaveTypeName = x.LeaveTypeName,
                    LeaveDays = x.LeaveDays
                })
                .ToList();
        }
        public async Task<ReportingManagerDto> GetReportingManagerAsync(int userId)
        {
            // Get logged-in user
            var user = (await _unitOfWork.Repository<User>()
                .GetAllAsync())
                .FirstOrDefault(x => x.UserId == userId);

            if (user == null)
                return new ReportingManagerDto();

            // Get manager using ReportingTo
            var manager = (await _unitOfWork.Repository<User>()
                .GetAllAsync())
                .FirstOrDefault(x => x.UserId == user.ReportingTo);

            return new ReportingManagerDto
            {
                UserId = user.UserId,
                EmployeeName = user.FullName,
                ManagerId = manager?.UserId,
                ManagerName = manager?.FullName,
                ManagerEmail = manager?.Email
            };
        }
        public async Task<int> SubmitLeaveAsync(LeaveRequestDto dto)
        {
            var entity = new LeaveRequest
            {
                UserId = dto.UserId,
                CompanyId = dto.CompanyId,
                RegionId = dto.RegionId,
                LeaveTypeId = dto.LeaveTypeId,
                IsHalfDay = dto.IsHalfDay,
                StartDate = DateOnly.FromDateTime(dto.StartDate),
                EndDate = DateOnly.FromDateTime(dto.EndDate),
                TotalDays = dto.TotalDays,
                Reason = dto.Reason,
                FileName = dto.FileName,
                FilePath = dto.FilePath,
                ReportingManagerId = dto.ReportingManagerId,
                Status = "Pending",
                AppliedDate = DateTime.Now,
                CreatedAt = DateTime.Now,
                CreatedBy = dto.UserId
            };

            await _unitOfWork.Repository<LeaveRequest>().AddAsync(entity);
            await _unitOfWork.CompleteAsync();

            return entity.LeaveRequestId;
        }

        public async Task<IEnumerable<LeaveRequestDto>> GetMyLeavesAsync(int userId)
        {
            var leaves = await _unitOfWork.Repository<LeaveRequest>()
                .FindAsync(x => x.UserId == userId);

            return leaves.Select(l => new LeaveRequestDto
            {
                LeaveRequestId = l.LeaveRequestId,
                LeaveTypeId = l.LeaveTypeId,
                LeaveTypeName = _unitOfWork.Repository<LeaveType>()
                            .GetAllAsync().Result
                            .FirstOrDefault(t => t.LeaveTypeId == l.LeaveTypeId)?.LeaveTypeName,

                // ✅ Send only DATE (no time)
                StartDate = l.StartDate.ToDateTime(TimeOnly.MinValue).Date,
                EndDate = l.EndDate.ToDateTime(TimeOnly.MinValue).Date,
                AppliedDate = l.AppliedDate.HasValue
                        ? l.AppliedDate.Value.Date
                        : null,

                TotalDays = l.TotalDays,
                Reason = l.Reason,
                FileName = l.FileName,
                FilePath = l.FilePath,
                Status = l.Status,
                IsHalfDay = (bool)l.IsHalfDay
            }).ToList();
        }

        public async Task<IEnumerable<LeaveRequestDto>> GetLeavesForManagerAsync(int managerId)
        {
            var leaves = await _unitOfWork.Repository<LeaveRequest>()
                .FindAsync(x => x.ReportingManagerId == managerId);

            var users = await _unitOfWork.Repository<User>().GetAllAsync();
            var leaveTypes = await _unitOfWork.Repository<LeaveType>().GetAllAsync();

            return leaves.Select(l => new LeaveRequestDto
            {
                LeaveRequestId = l.LeaveRequestId,
                UserId = l.UserId,
                EmployeeName = users.FirstOrDefault(u => u.UserId == l.UserId)?.FullName,
                LeaveTypeName = leaveTypes.FirstOrDefault(t => t.LeaveTypeId == l.LeaveTypeId)?.LeaveTypeName,
                StartDate = l.StartDate.ToDateTime(TimeOnly.MinValue).Date,
                EndDate = l.EndDate.ToDateTime(TimeOnly.MinValue).Date,
                TotalDays = l.TotalDays,
                Reason = l.Reason,
                Status = l.Status,
                FileName = l.FileName,
                FilePath = l.FilePath
            }).ToList();
        }
        public async Task<bool> ApproveLeaveFromEmailAsync(int leaveId)
        {
            var repo = _unitOfWork.Repository<LeaveRequest>();
            var leave = await repo.GetByIdAsync(leaveId);

            if (leave == null) return false;

            leave.Status = "Approved";
            leave.ApprovedRejectedDate = DateTime.Now;

            repo.Update(leave);
            await _unitOfWork.CompleteAsync();

            // ✅ Send email to employee
            var employee = await _unitOfWork.Repository<User>()
                .GetByIdAsync(leave.UserId);

            if (employee != null)
            {
                string subject = "Leave Approved";
                string body = $"Hello {employee.FullName},<br/><br/>Your leave has been <b>approved</b>.";

                await _emailService.SendEmailAsync(employee.Email, subject, body);
            }

            return true;
        }
        public async Task<bool> RejectLeaveFromEmailAsync(int leaveId)
        {
            var repo = _unitOfWork.Repository<LeaveRequest>();
            var leave = await repo.GetByIdAsync(leaveId);

            if (leave == null) return false;

            leave.Status = "Rejected";
            leave.ApprovedRejectedDate = DateTime.Now;

            repo.Update(leave);
            await _unitOfWork.CompleteAsync();

            // ✅ Send email to employee
            var employee = await _unitOfWork.Repository<User>()
                .GetByIdAsync(leave.UserId);

            if (employee != null)
            {
                string subject = "Leave Rejected";
                string body = $"Hello {employee.FullName},<br/><br/>Your leave has been <b>rejected</b>.";

                await _emailService.SendEmailAsync(employee.Email, subject, body);
            }

            return true;
        }
        public async Task SendLeaveEmailToManagerAsync(int leaveRequestId)
        {
            var leave = await _unitOfWork.Repository<LeaveRequest>()
                .GetByIdAsync(leaveRequestId);

            if (leave == null || leave.ReportingManagerId == null)
                return;

            var manager = await _unitOfWork.Repository<User>()
                .GetByIdAsync(leave.ReportingManagerId.Value);

            var employee = await _unitOfWork.Repository<User>()
                .GetByIdAsync(leave.UserId);

            var leaveType = await _unitOfWork.Repository<LeaveType>()
                .GetByIdAsync(leave.LeaveTypeId);

            if (manager == null || employee == null)
                return;

            string portalUrl = _configuration["AppSettings:PortalUrl"];

            string approveUrl = $"{portalUrl}/api/Leave/ApproveFromEmail/{leaveRequestId}";
            string rejectUrl = $"{portalUrl}/api/Leave/RejectFromEmail/{leaveRequestId}";

            string subject = "New Leave Request";

            string body = $@"
        <html>
        <body style='font-family:Segoe UI'>
            <h3>Leave Request</h3>
            <p><b>Employee:</b> {employee.FullName}</p>
            <p><b>Leave Type:</b> {leaveType?.LeaveTypeName}</p>
            <p><b>From:</b> {leave.StartDate}</p>
            <p><b>To:</b> {leave.EndDate}</p>
            <p><b>Total Days:</b> {leave.TotalDays}</p>
            <p><b>Reason:</b> {leave.Reason}</p>
            <br/>
            <a href='{approveUrl}' style='padding:10px;background:green;color:white;text-decoration:none;'>Approve</a>
            &nbsp;
            <a href='{rejectUrl}' style='padding:10px;background:red;color:white;text-decoration:none;'>Reject</a>
        </body>
        </html>";

            await _emailService.SendEmailAsync(manager.Email, subject, body);
        }

        // ✅ SINGLE APPROVE
        public async Task<bool> ApproveLeaveByManagerAsync(int leaveId)
        {
            var repo = _unitOfWork.Repository<LeaveRequest>();
            var leave = await repo.GetByIdAsync(leaveId);
            if (leave == null) return false;

            leave.Status = "Approved";
            leave.ApprovedRejectedDate = DateTime.Now;

            repo.Update(leave);
            await _unitOfWork.CompleteAsync();

            var user = await _unitOfWork.Repository<User>().GetByIdAsync(leave.UserId);
            if (user != null)
            {
                await _emailService.SendEmailAsync(
                    user.Email,
                    "Leave Approved",
                    $"Hello {user.FullName},<br>Your leave has been <b>approved</b>.");
            }

            return true;
        }

        // ✅ SINGLE REJECT
        public async Task<bool> RejectLeaveByManagerAsync(int leaveId)
        {
            var repo = _unitOfWork.Repository<LeaveRequest>();
            var leave = await repo.GetByIdAsync(leaveId);
            if (leave == null) return false;

            leave.Status = "Rejected";
            leave.ApprovedRejectedDate = DateTime.Now;

            repo.Update(leave);
            await _unitOfWork.CompleteAsync();

            var user = await _unitOfWork.Repository<User>().GetByIdAsync(leave.UserId);
            if (user != null)
            {
                await _emailService.SendEmailAsync(
                    user.Email,
                    "Leave Rejected",
                    $"Hello {user.FullName},<br>Your leave has been <b>rejected</b>.");
            }

            return true;
        }

        // ✅ BULK APPROVE
        public async Task<bool> BulkApproveLeavesAsync(List<int> leaveIds)
        {
            foreach (var id in leaveIds)
                await ApproveLeaveByManagerAsync(id);

            return true;
        }

        // ✅ BULK REJECT
        public async Task<bool> BulkRejectLeavesAsync(List<int> leaveIds)
        {
            foreach (var id in leaveIds)
                await RejectLeaveByManagerAsync(id);

            return true;
        }
        public async Task<IEnumerable<LeaveRequestDto>> GetLeavesForUserAsync(int userId)
        {
            var leaves = await _unitOfWork.Repository<LeaveRequest>()
                .FindAsync(x => x.UserId == userId);

            var users = await _unitOfWork.Repository<User>().GetAllAsync();
            var leaveTypes = await _unitOfWork.Repository<LeaveType>().GetAllAsync();

            return leaves.Select(l => new LeaveRequestDto
            {
                LeaveRequestId = l.LeaveRequestId,
                UserId = l.UserId,
                EmployeeName = users.FirstOrDefault(u => u.UserId == l.UserId)?.FullName,
                LeaveTypeName = leaveTypes.FirstOrDefault(t => t.LeaveTypeId == l.LeaveTypeId)?.LeaveTypeName,
                StartDate = l.StartDate.ToDateTime(TimeOnly.MinValue).Date,
                EndDate = l.EndDate.ToDateTime(TimeOnly.MinValue).Date,
                TotalDays = (decimal)l.TotalDays,
                Status = l.Status
            }).ToList();
        }

        public async Task<IEnumerable<LeaveRequestDto>> GetLeavesForManagerUserAsync(int managerId)
        {
            var employees = (await _unitOfWork.Repository<User>()
                .FindAsync(u => u.ReportingTo == managerId))
                .Select(e => e.UserId)
                .ToList();

            var leaves = await _unitOfWork.Repository<LeaveRequest>()
                .FindAsync(l => employees.Contains(l.UserId));

            var users = await _unitOfWork.Repository<User>().GetAllAsync();
            var leaveTypes = await _unitOfWork.Repository<LeaveType>().GetAllAsync();

            return leaves.Select(l => new LeaveRequestDto
            {
                LeaveRequestId = l.LeaveRequestId,
                UserId = l.UserId,
                EmployeeName = users.FirstOrDefault(u => u.UserId == l.UserId)?.FullName,
                LeaveTypeName = leaveTypes.FirstOrDefault(t => t.LeaveTypeId == l.LeaveTypeId)?.LeaveTypeName,
                StartDate = l.StartDate.ToDateTime(TimeOnly.MinValue).Date,
                EndDate = l.EndDate.ToDateTime(TimeOnly.MinValue).Date,
                TotalDays = (decimal)l.TotalDays,
                Status = l.Status
            }).ToList();
        }

    }
}
