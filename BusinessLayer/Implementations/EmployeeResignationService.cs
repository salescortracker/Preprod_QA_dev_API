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
    public class EmployeeResignationService: IEmployeeResignationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmailService _emailService;


        public EmployeeResignationService(IUnitOfWork unitOfWork, IEmailService emailService)
        {
            _unitOfWork = unitOfWork;
            _emailService = emailService;
        }

        // ===================== OLD REQUIRED METHOD =====================
        public async Task<IEnumerable<EmployeeResignationDto>> GetAllResignationsAsync()
        {
            var list = await _unitOfWork.Repository<EmployeeResignation>().GetAllAsync();
            return list.Select(MapToDto);
        }

        // ===================== OLD REQUIRED METHOD =====================
        public async Task<IEnumerable<EmployeeResignationDto>> SearchResignationsAsync(object filter)
        {
            var props = filter.GetType().GetProperties();
            var all = await _unitOfWork.Repository<EmployeeResignation>().GetAllAsync();
            var query = all.AsQueryable();

            foreach (var prop in props)
            {
                var name = prop.Name;
                var value = prop.GetValue(filter);
                if (value == null) continue;

                switch (name)
                {
                    case nameof(EmployeeResignation.EmployeeId):
                        query = query.Where(x => x.EmployeeId != null && x.EmployeeId.Contains(value.ToString()!));
                        break;

                    case nameof(EmployeeResignation.ResignationType):
                        query = query.Where(x => x.ResignationType != null && x.ResignationType.Contains(value.ToString()!));
                        break;

                    case nameof(EmployeeResignation.Status):
                        query = query.Where(x => x.Status != null &&
                            x.Status.Equals(value.ToString(), StringComparison.OrdinalIgnoreCase));
                        break;

                    case nameof(EmployeeResignation.CompanyId):
                        query = query.Where(x => x.CompanyId == Convert.ToInt32(value));
                        break;

                    case nameof(EmployeeResignation.RegionId):
                        query = query.Where(x => x.RegionId == Convert.ToInt32(value));
                        break;
                }
            }

            return query.Select(MapToDto).ToList();
        }

        // ===================== OLD REQUIRED METHOD (Bulk Insert) =====================
        public async Task<IEnumerable<EmployeeResignation>> AddMultipleResignationsAsync(List<EmployeeResignationDto> dtos)
        {
            var entities = dtos.Select(dto => new EmployeeResignation
            {
                EmployeeId = dto.EmployeeId,
                ResignationType = dto.ResignationType,
                NoticePeriod = dto.NoticePeriod,
                LastWorkingDay = dto.LastWorkingDay,
                ResignationReason = dto.ResignationReason,
                Status = dto.Status,
                CreatedBy = dto.CreatedBy,
                CreatedAt = DateTime.Now,
                CompanyId = dto.CompanyId,
                RegionId = dto.RegionId,
                UserId = dto.UserId
            }).ToList();

            await _unitOfWork.Repository<EmployeeResignation>().AddRangeAsync(entities);
            await _unitOfWork.CompleteAsync();

            return entities;
        }

        // ===================== NEW FILTERED GET ALL =====================
        public async Task<IEnumerable<EmployeeResignationDto>> GetResignationsByCompanyRegionAsync(int companyId, int regionId)
        {
            var list = await _unitOfWork.Repository<EmployeeResignation>().GetAllAsync();
            return list
                .Where(e => e.CompanyId == companyId && e.RegionId == regionId)
                .Select(MapToDto)
                .ToList();
        }

        // ===================== NEW FILTERED GET BY ID =====================
        public async Task<EmployeeResignationDto?> GetResignationByIdFilteredAsync(int id, int companyId, int regionId)
        {
            var entity = await _unitOfWork.Repository<EmployeeResignation>().GetByIdAsync(id);

            if (entity == null) return null;
            if (entity.CompanyId != companyId || entity.RegionId != regionId) return null;

            return MapToDto(entity);
        }

        // ===================== GET BY ID =====================
        public async Task<EmployeeResignationDto?> GetResignationByIdAsync(int id)
        {
            var entity = await _unitOfWork.Repository<EmployeeResignation>().GetByIdAsync(id);
            return entity == null ? null : MapToDto(entity);
        }
        private async Task<User?> GetManagerAsync(int userId)
        {
            var users = await _unitOfWork.Repository<User>().GetAllAsync();
            var employee = users.FirstOrDefault(u => u.UserId == userId);

            if (employee?.ReportingTo == null) return null;

            return users.FirstOrDefault(u => u.UserId == employee.ReportingTo);
        }
        private async Task<List<User>> GetHrUsersAsync(int companyId, int regionId)
        {
            var users = await _unitOfWork.Repository<User>().GetAllAsync();

            return users
                .Where(u =>
                    u.CompanyId == companyId &&
                    u.RegionId == regionId &&
                    u.RoleId == 4 &&                 // HR ROLE
                    u.Status == "Active"             // ✅ already exists
                )
                .ToList();
        }


        // ===================== CREATE =====================
        public async Task<EmployeeResignationDto> AddResignationAsync(EmployeeResignationDto dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            if (!dto.UserId.HasValue || !dto.CompanyId.HasValue || !dto.RegionId.HasValue)
                throw new Exception("UserId, CompanyId and RegionId are required.");

            var entity = new EmployeeResignation
            {
                EmployeeId = dto.EmployeeId,
                ResignationType = dto.ResignationType,
                NoticePeriod = dto.NoticePeriod,
                LastWorkingDay = dto.LastWorkingDay,
                ResignationReason = dto.ResignationReason,
                Status = "Pending",
                CreatedBy = dto.CreatedBy,
                CreatedAt = DateTime.UtcNow,
                CompanyId = dto.CompanyId.Value,
                RegionId = dto.RegionId.Value,
                UserId = dto.UserId.Value,
                RoleId = dto.RoleId
            };

            await _unitOfWork.Repository<EmployeeResignation>().AddAsync(entity);
            await _unitOfWork.CompleteAsync();

            // ================= EMAIL SECTION =================

            var manager = await GetManagerAsync(dto.UserId.Value);

            // ❌ No manager OR no manager email → no mail
            if (manager == null || string.IsNullOrWhiteSpace(manager.Email))
                return MapToDto(entity);

            // HR users for CC
            var hrUsers = await GetHrUsersAsync(dto.CompanyId.Value, dto.RegionId.Value);

            var hrCcEmails = hrUsers
                .Where(x => !string.IsNullOrWhiteSpace(x.Email))
                .Select(x => x.Email)
                .Distinct()
                .ToList();

            var subject = $"Resignation Submitted - {dto.EmployeeId}";

            var managerName = string.IsNullOrWhiteSpace(manager.FullName)
                ? "Manager"
                : manager.FullName;

            var body = $@"
        <p>Dear {managerName},</p>

        <p>Your team member has submitted a resignation request.</p>

        <table cellpadding='6' cellspacing='0'>
            <tr><td><b>Employee Code</b></td><td>: {dto.EmployeeId}</td></tr>
            <tr><td><b>Resignation Type</b></td><td>: {dto.ResignationType}</td></tr>
            <tr><td><b>Notice Period</b></td><td>: {dto.NoticePeriod} days</td></tr>
            <tr><td><b>Last Working Day</b></td><td>: {dto.LastWorkingDay:dd-MMM-yyyy}</td></tr>
            <tr><td><b>Reason</b></td><td>: {dto.ResignationReason}</td></tr>
        </table>

        <br/>
        <p>Please login to <b>HRMS</b> to review and take action.</p>

        <br/>
        <p>Thanks & Regards,<br/>
        <b>Cortracker HRMS</b></p>
    ";

            // ✅ ONE EMAIL ONLY
            // TO  → Reporting Manager
            // CC  → HR
            await _emailService.SendEmailAsync(
                manager.Email,
                subject,
                body,
                hrCcEmails
            );

            return MapToDto(entity);
        }


        // ===================== UPDATE =====================
        public async Task<EmployeeResignationDto> UpdateResignationAsync(int id, EmployeeResignationDto dto)
        {
            var entity = await _unitOfWork.Repository<EmployeeResignation>().GetByIdAsync(id);
            if (entity == null) throw new Exception("Record not found.");

            if (entity.CompanyId != dto.CompanyId || entity.RegionId != dto.RegionId)
                throw new Exception("Not allowed to update this record.");

            entity.EmployeeId = dto.EmployeeId;
            entity.ResignationType = dto.ResignationType;
            entity.NoticePeriod = dto.NoticePeriod;
            entity.LastWorkingDay = dto.LastWorkingDay;
            entity.ResignationReason = dto.ResignationReason;
            entity.Status = dto.Status;
            entity.ModifiedBy = dto.ModifiedBy;
            entity.ModifiedAt = DateTime.UtcNow;
            if (entity.Status == "Approved by Manager")
                entity.ManagerApprovedDate = DateTime.UtcNow;
            if (entity.Status == "Rejected by Manager")
                entity.ManagerRejectedDate = DateTime.UtcNow;
            _unitOfWork.Repository<EmployeeResignation>().Update(entity);
            await _unitOfWork.CompleteAsync();

            return MapToDto(entity);
        }

        // ===================== DELETE =====================
        public async Task<bool> DeleteResignationAsync(int id, int companyId, int regionId)
        {
            var entity = await _unitOfWork.Repository<EmployeeResignation>().GetByIdAsync(id);

            if (entity == null) return false;
            if (entity.CompanyId != companyId || entity.RegionId != regionId) return false;

            _unitOfWork.Repository<EmployeeResignation>().Remove(entity);
            await _unitOfWork.CompleteAsync();

            return true;
        }

        // ===================== MAPPER =====================
        private EmployeeResignationDto MapToDto(EmployeeResignation e)
        {
            return new EmployeeResignationDto
            {
                ResignationId = e.ResignationId,
                EmployeeId = e.EmployeeId,
                ResignationType = e.ResignationType,
                NoticePeriod = e.NoticePeriod,
                LastWorkingDay = e.LastWorkingDay,
                ResignationReason = e.ResignationReason,
                Status = e.Status,

                CompanyId = e.CompanyId,
                RegionId = e.RegionId,
                UserId = e.UserId,

                // ✅ ADD THESE
                ManagerReason = e.ManagerReason,
                ManagerApprovedDate = e.ManagerApprovedDate,
                ManagerRejectedDate = e.ManagerRejectedDate,

                // ✅ ADD THESE
                HRReason = e.HrReason,
                HRApprovedDate = e.HrApprovedDate,
                HRRejectedDate = e.HrRejectedDate

            };
        }

        public async Task<bool> UpdateResignationStatusAsync(
      int resignationId,
      string status,
      string? managerReason,
      bool isManagerApprove,
      bool isManagerReject,
          string? hrReason = null,
    bool isHRApprove = false,
    bool isHRReject = false)
        {
            var entity = await _unitOfWork.Repository<EmployeeResignation>()
                .GetByIdAsync(resignationId);

            if (entity == null) return false;

            entity.Status = status;
            entity.ManagerReason = managerReason;
            entity.ModifiedAt = DateTime.UtcNow;

            if (isManagerApprove)
                entity.ManagerApprovedDate = DateTime.UtcNow;

            if (isManagerReject)
                entity.ManagerRejectedDate = DateTime.UtcNow;
            if (isHRApprove)
            {
                entity.HrReason = hrReason;
                entity.HrApprovedDate = DateTime.UtcNow;
            }

            if (isHRReject)
            {
                entity.HrReason = hrReason;
                entity.HrRejectedDate = DateTime.UtcNow;
            }

            _unitOfWork.Repository<EmployeeResignation>().Update(entity);
            await _unitOfWork.CompleteAsync();

            // send email
            if (entity.UserId.HasValue)
            {
                var employee = await _unitOfWork.Repository<User>()
                    .GetByIdAsync(entity.UserId.Value);

                if (employee != null && !string.IsNullOrWhiteSpace(employee.Email))
                {
                    List<User> hrUsers = new();
                    if (entity.CompanyId.HasValue && entity.RegionId.HasValue)
                    {
                        hrUsers = await GetHrUsersAsync(entity.CompanyId.Value, entity.RegionId.Value);
                    }

                    var hrCc = hrUsers
                        .Where(x => !string.IsNullOrWhiteSpace(x.Email))
                        .Select(x => x.Email)
                        .ToList();

                    var subject = isManagerApprove
                        ? $"Resignation Approved - {entity.EmployeeId}"
                        : $"Resignation Rejected - {entity.EmployeeId}";

                    var body = $@"
<p>Dear {employee.FullName},</p>
<p>Your resignation has been <b>{(isManagerApprove ? "APPROVED" : "REJECTED")}</b> by your manager.</p>
<p><b>Manager Comments:</b><br/>{managerReason}</p>
<p>Status: <b>{entity.Status}</b></p>
<br/>
<p>Regards,<br/><b>HRMS</b></p>";

                    await _emailService.SendEmailAsync(employee.Email, subject, body, hrCc);
                }
            }

            return true;
        }


        public async Task<IEnumerable<EmployeeResignationDto>> GetResignationsForReportingManagerAsync(int managerUserId)
        {
            var employees = await _unitOfWork.Repository<User>()
                .FindAsync(u => u.ReportingTo == managerUserId && u.Status == "Active");

            var employeeUserIds = employees.Select(x => x.UserId).ToList();

            if (!employeeUserIds.Any())
                return Enumerable.Empty<EmployeeResignationDto>();

            var resignations = await _unitOfWork.Repository<EmployeeResignation>()
                .FindAsync(r =>
                    r.UserId.HasValue &&
                    employeeUserIds.Contains(r.UserId.Value)
                ); // ✅ NO STATUS FILTER

            return resignations
                .OrderByDescending(r => r.CreatedAt)
                .Select(MapToDto);
        }

        public async Task<IEnumerable<EmployeeResignationDto>> GetResignationsForHRAsync(
    int companyId,
    int regionId)
        {
            var resignations = await _unitOfWork.Repository<EmployeeResignation>()
                .FindAsync(r =>
                    r.CompanyId == companyId &&
                    r.RegionId == regionId &&
                    (
                        r.Status == "Approved" ||          // Manager approved
                        r.Status == "Rejected" ||          // (optional)
                        r.Status == "HR Approved" ||
                        r.Status == "HR Rejected"
                    )
                );

            return resignations
                .OrderByDescending(r => r.CreatedAt)
                .Select(MapToDto);
        }
    }
}
