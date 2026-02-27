using BusinessLayer.DTOs;
using BusinessLayer.Interfaces;
using DataAccessLayer.DBContext;
using DataAccessLayer.Repositories.GeneralRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Implementations
{
    public class TimesheetService:ITimesheetService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmailService _emailService;

        public TimesheetService(IUnitOfWork unitOfWork, IEmailService emailService)
        {
            _unitOfWork = unitOfWork;
            _emailService = emailService;
        }

        public async Task<LoggedInUserDto> GetLoggedInUserAsync(int userId)
        {
            var user = (await _unitOfWork.Repository<User>().GetAllAsync())
                .FirstOrDefault(x => x.UserId == userId);

            if (user == null)
                return new LoggedInUserDto();

            return new LoggedInUserDto
            {
                UserId = user.UserId,
                EmployeeName = user.FullName,
                EmployeeCode = user.EmployeeCode
            };
        }

        public async Task<int> SaveTimesheetAsync(TimesheetRequestDto dto)
        {
            var user = await _unitOfWork.Repository<User>().GetByIdAsync(dto.UserId);

            var timesheet = new Timesheet
            {
                UserId = dto.UserId,
                ManagerUserId = user?.ReportingTo,
                CompanyId = dto.CompanyId,
                RegionId = dto.RegionId,
                EmployeeCode = dto.EmployeeCode,
                EmployeeName = dto.EmployeeName,
                TimesheetDate = DateOnly.FromDateTime(dto.TimesheetDate),
                Comments = dto.Comments,
                FileName = dto.FileName ?? "",
                FilePath = dto.FilePath,
                Status = "Pending",
                CreatedBy = dto.UserId,
                CreatedAt = DateTime.Now
            };

            await _unitOfWork.Repository<Timesheet>().AddAsync(timesheet);
            await _unitOfWork.CompleteAsync();

            foreach (var p in dto.Projects)
            {
                var project = new TimesheetProject
                {
                    TimesheetId = timesheet.TimesheetId,
                    ProjectName = p.ProjectName,
                    StartTime = TimeOnly.Parse(p.StartTime),
                    EndTime = TimeOnly.Parse(p.EndTime),

                    TotalMinutes = p.TotalMinutes,
                    TotalHoursText = p.TotalHoursText ?? "0 Hours",

                    Otminutes = p.OTMinutes.HasValue && p.OTMinutes.Value >= 0 ? p.OTMinutes.Value : 0,
                    OthoursText = p.OTHoursText ?? "0 Hours",

                    CreatedAt = DateTime.Now
                };


                await _unitOfWork.Repository<TimesheetProject>().AddAsync(project);
            }

            await _unitOfWork.CompleteAsync();
            return timesheet.TimesheetId;
        }

        public async Task<IEnumerable<TimesheetListDto>> GetMyTimesheetsAsync(int userId)
        {
            var timesheets = await _unitOfWork.Repository<Timesheet>()
                .FindAsync(x => x.UserId == userId);

            var timesheetIds = timesheets.Select(t => t.TimesheetId).ToList();

            var projects = await _unitOfWork.Repository<TimesheetProject>()
                .FindAsync(p => timesheetIds.Contains(p.TimesheetId));

            return timesheets.Select(t => new TimesheetListDto
            {
                TimesheetId = t.TimesheetId,
                EmployeeName = t.EmployeeName,
                EmployeeCode = t.EmployeeCode,
                TimesheetDate = t.TimesheetDate.ToDateTime(TimeOnly.MinValue),
                Status = t.Status,

                Projects = projects
                    .Where(p => p.TimesheetId == t.TimesheetId)
                    .Select(p => new TimesheetProjectDto
                    {
                        ProjectName = p.ProjectName,
                        StartTime = p.StartTime.ToString(),
                        EndTime = p.EndTime.ToString(),
                        TotalMinutes = p.TotalMinutes,
                        TotalHoursText = p.TotalHoursText,
                        OTMinutes = p.Otminutes,
                        OTHoursText = string.IsNullOrEmpty(p.OthoursText) ? "0 Hours" : p.OthoursText

                    }).ToList()
            });
        }

        // ✅ SEND SELECTED TIMESHEETS + EMAIL MANAGER
        public async Task<bool> SendSelectedTimesheetsAsync(List<int> timesheetIds)
        {
            var timesheets = await _unitOfWork.Repository<Timesheet>()
                .FindAsync(x => timesheetIds.Contains(x.TimesheetId));

            foreach (var ts in timesheets)
            {
                ts.Status = "Submitted";
                ts.ModifiedAt = DateTime.Now;

                await SendManagerEmailAsync(ts);
            }

            await _unitOfWork.CompleteAsync();
            return true;
        }

        // ✅ EMAIL MANAGER LOGIC
        private async Task SendManagerEmailAsync(Timesheet ts)
        {
            if (!ts.ManagerUserId.HasValue)
                return;

            var manager = await _unitOfWork.Repository<User>()
                .GetByIdAsync(ts.ManagerUserId.Value);

            if (manager == null || string.IsNullOrEmpty(manager.Email))
                return;

            string subject = $"Timesheet Submitted - {ts.EmployeeName}";

            string body = $@"
            <html>
            <body style='font-family:Segoe UI'>
                <h3>Timesheet Submitted</h3>
                <p><b>Employee:</b> {ts.EmployeeName} ({ts.EmployeeCode})</p>
                <p><b>Date:</b> {ts.TimesheetDate:dd-MMM-yyyy}</p>
                <p><b>Status:</b> Submitted</p>
                <p><b>Comments:</b> {ts.Comments}</p>
                <hr/>
                <p>Please login to HRMS to review the timesheet.</p>
            </body>
            </html>";

            await _emailService.SendEmailAsync(manager.Email, subject, body);
        }

        public async Task<IEnumerable<ManagerTimesheetDto>> GetTimesheetsForManagerAsync(int managerUserId)
        {
            var timesheets = await _unitOfWork.Repository<Timesheet>()
                .FindAsync(t =>
                    t.ManagerUserId == managerUserId
                    && (t.Status == "Pending" || t.Status == "Submitted")
                );

            var timesheetIds = timesheets.Select(t => t.TimesheetId).ToList();

            var projects = await _unitOfWork.Repository<TimesheetProject>()
                .FindAsync(p => timesheetIds.Contains(p.TimesheetId));

            return timesheets.Select(t => new ManagerTimesheetDto
            {
                TimesheetId = t.TimesheetId,
                UserId = t.UserId,
                EmployeeName = t.EmployeeName,
                EmployeeCode = t.EmployeeCode,
                TimesheetDate = t.TimesheetDate.ToDateTime(TimeOnly.MinValue),
                Status = t.Status,
                Comments = t.Comments,

                Projects = projects
                    .Where(p => p.TimesheetId == t.TimesheetId)
                    .Select(p => new TimesheetProjectDto
                    {
                        ProjectName = p.ProjectName,
                        StartTime = p.StartTime.ToString(),
                        EndTime = p.EndTime.ToString(),
                        TotalMinutes = p.TotalMinutes,
                        TotalHoursText = p.TotalHoursText,
                        OTMinutes = p.Otminutes,
                        OTHoursText = p.OthoursText ?? "0 Hours"
                    }).ToList()
            });
        }
        public async Task<ManagerTimesheetDto> GetTimesheetDetailAsync(int timesheetId)
        {
            var ts = await _unitOfWork.Repository<Timesheet>()
                .GetByIdAsync(timesheetId);

            if (ts == null) return null;

            var projects = await _unitOfWork.Repository<TimesheetProject>()
                .FindAsync(p => p.TimesheetId == timesheetId);

            return new ManagerTimesheetDto
            {
                TimesheetId = ts.TimesheetId,
                UserId = ts.UserId,
                EmployeeName = ts.EmployeeName,
                EmployeeCode = ts.EmployeeCode,
                TimesheetDate = ts.TimesheetDate.ToDateTime(TimeOnly.MinValue),
                Status = ts.Status,
                Comments = ts.Comments,
                Projects = projects.Select(p => new TimesheetProjectDto
                {
                    ProjectName = p.ProjectName,
                    StartTime = p.StartTime.ToString(),
                    EndTime = p.EndTime.ToString(),
                    TotalMinutes = p.TotalMinutes,
                    TotalHoursText = p.TotalHoursText,
                    OTMinutes = p.Otminutes,
                    OTHoursText = p.OthoursText
                }).ToList()
            };
        }
        public async Task<bool> ApproveTimesheetsAsync(List<int> ids, string comments)
        {
            var timesheets = await _unitOfWork.Repository<Timesheet>()
                .FindAsync(x => ids.Contains(x.TimesheetId));

            foreach (var ts in timesheets)
            {
                ts.Status = "Approved";
                ts.Comments = comments;
                ts.ModifiedAt = DateTime.Now;

                await SendEmployeeStatusEmailAsync(ts, "Approved");
            }

            await _unitOfWork.CompleteAsync();
            return true;
        }

        public async Task<bool> RejectTimesheetsAsync(List<int> ids, string comments)
        {
            var timesheets = await _unitOfWork.Repository<Timesheet>()
                .FindAsync(x => ids.Contains(x.TimesheetId));

            foreach (var ts in timesheets)
            {
                ts.Status = "Rejected";
                ts.Comments = comments;
                ts.ModifiedAt = DateTime.Now;

                await SendEmployeeStatusEmailAsync(ts, "Rejected");
            }

            await _unitOfWork.CompleteAsync();
            return true;
        }
        private async Task SendEmployeeStatusEmailAsync(Timesheet ts, string status)
        {
            var employee = await _unitOfWork.Repository<User>()
                .GetByIdAsync(ts.UserId);

            if (employee == null || string.IsNullOrEmpty(employee.Email))
                return;

            string subject = $"Timesheet {status} - {ts.TimesheetDate:dd-MMM-yyyy}";

            string body = $@"
    <html>
    <body style='font-family:Segoe UI'>
        <h3>Your Timesheet Has Been {status}</h3>
        <p><b>Employee:</b> {ts.EmployeeName} ({ts.EmployeeCode})</p>
        <p><b>Date:</b> {ts.TimesheetDate:dd-MMM-yyyy}</p>
        <p><b>Status:</b> {status}</p>
        <p><b>Manager Comments:</b> {ts.Comments}</p>
        <hr/>
        <p>Please login to HRMS for details.</p>
    </body>
    </html>";

            await _emailService.SendEmailAsync(employee.Email, subject, body);
        }

    }
}
