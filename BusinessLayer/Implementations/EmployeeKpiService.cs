using BusinessLayer.DTOs;
using BusinessLayer.Interfaces;
using DataAccessLayer.DBContext;
using Microsoft.EntityFrameworkCore;

namespace BusinessLayer.Implementations
{
    public class EmployeeKpiService:IEmployeeKpiService
    {
        private readonly HRMSContext _context;   // UPDATED

        public EmployeeKpiService(HRMSContext context)   // UPDATED
        {
            _context = context;
        }

        // CREATE KPI
        public async Task<int> CreateKpiAsync(EmployeeKpiDto dto)
        {
            var kpi = new EmployeeKpi
            {
                UserId = dto.UserId,
                CompanyId = dto.CompanyId,
                RegionId = dto.RegionId,
                EmployeeNameId = dto.EmployeeNameId,
                ReportingManagerId = dto.ReportingManagerId,
                Designation = dto.Designation,
                DepartmentId = dto.DepartmentId,
                DateOfJoining = dto.DateOfJoining,
                ProbationStatus = dto.ProbationStatus,
                PerformanceCycle = dto.PerformanceCycle,
                ApplicableStartDate = dto.ApplicableStartDate,
                ApplicableEndDate = dto.ApplicableEndDate,
                ProgressType = dto.ProgressType,
                AppraisalYear = dto.AppraisalYear,
                DocumentEvidencePath = dto.DocumentEvidencePath, // relative path only
                SelfReviewSummary = dto.SelfReviewSummary,

                // Auditing
                CreatedBy = dto.CreatedBy,
                CreatedAt = DateTime.Now
            };

            _context.EmployeeKpis.Add(kpi);
            await _context.SaveChangesAsync();

            foreach (var item in dto.KpiItems)
            {
                _context.EmployeeKpiitems.Add(new EmployeeKpiitem
                {
                    Kpiid = kpi.Kpiid,
                    Kpiobjective = item.KpiObjective,
                    Weightage = item.Weightage,
                    Target = item.Target,
                    TaskCompleted = item.TaskCompleted,
                    SelfRating = item.SelfRating,
                    Remarks = item.Remarks,

                    CreatedBy = dto.CreatedBy,
                    CreatedAt = DateTime.Now
                });
            }

            await _context.SaveChangesAsync();
            return kpi.Kpiid;
        }

        // GET ALL BY USER
        public async Task<List<EmployeeKpiDto>> GetKpisByUserAsync(int userId)
        {
            var list = await _context.EmployeeKpis
                .Include(x => x.EmployeeKpiitems)
                .Where(x => x.UserId == userId)
                .ToListAsync();

            return list.Select(MapToDto).ToList();
        }

        // GET KPI ALL
        public async Task<List<EmployeeKpiDto>> GetAllKpisAsync()
        {
            var list = await _context.EmployeeKpis
                .Include(x => x.EmployeeKpiitems)
                .ToListAsync();

            return list.Select(MapToDto).ToList();
        }
        // UPDATE KPI
        public async Task<bool> UpdateKpiAsync(EmployeeKpiDto dto)
        {
            var kpi = await _context.EmployeeKpis
                .Include(x => x.EmployeeKpiitems)
                .FirstOrDefaultAsync(x => x.Kpiid == dto.KpiId);

            if (kpi == null) return false;

            // Update main fields
            kpi.Designation = dto.Designation;
            kpi.DepartmentId = dto.DepartmentId;
            kpi.DateOfJoining = dto.DateOfJoining;
            kpi.ProbationStatus = dto.ProbationStatus;
            kpi.PerformanceCycle = dto.PerformanceCycle;
            kpi.ApplicableStartDate = dto.ApplicableStartDate;
            kpi.ApplicableEndDate = dto.ApplicableEndDate;
            kpi.ProgressType = dto.ProgressType;
            kpi.AppraisalYear = dto.AppraisalYear;
            if (!string.IsNullOrEmpty(dto.DocumentEvidencePath))
                kpi.DocumentEvidencePath = dto.DocumentEvidencePath; // update relative path only
            kpi.SelfReviewSummary = dto.SelfReviewSummary;

            // Auditing
            kpi.ModifiedBy = dto.ModifiedBy;
            kpi.ModifiedAt = DateTime.Now;

            // Remove existing items
            _context.EmployeeKpiitems.RemoveRange(kpi.EmployeeKpiitems);

            // Add new items
            foreach (var item in dto.KpiItems)
            {
                _context.EmployeeKpiitems.Add(new EmployeeKpiitem
                {
                    Kpiid = dto.KpiId,
                    Kpiobjective = item.KpiObjective,
                    Weightage = item.Weightage,
                    Target = item.Target,
                    TaskCompleted = item.TaskCompleted,
                    SelfRating = item.SelfRating,
                    Remarks = item.Remarks,

                    CreatedBy = dto.ModifiedBy,
                    CreatedAt = DateTime.Now
                });
            }

            await _context.SaveChangesAsync();
            return true;
        }


        // DELETE
        public async Task<bool> DeleteKpiAsync(int kpiId)
        {
            var kpi = await _context.EmployeeKpis
                .FirstOrDefaultAsync(x => x.Kpiid == kpiId);

            if (kpi == null) return false;

            _context.EmployeeKpis.Remove(kpi);

            var items = _context.EmployeeKpiitems.Where(x => x.Kpiid == kpiId);
            _context.EmployeeKpiitems.RemoveRange(items);

            await _context.SaveChangesAsync();
            return true;
        }

        // MAPPER
        private EmployeeKpiDto MapToDto(EmployeeKpi k)
        {
            return new EmployeeKpiDto
            {
                KpiId = k.Kpiid,
                UserId = k.UserId,
                CompanyId = k.CompanyId,
                RegionId = k.RegionId,
                EmployeeNameId = k.EmployeeNameId,
                ReportingManagerId = k.ReportingManagerId,
                Designation = k.Designation,
                DepartmentId = k.DepartmentId,
                DateOfJoining = k.DateOfJoining,
                ProbationStatus = k.ProbationStatus,
                PerformanceCycle = k.PerformanceCycle,
                ApplicableStartDate = k.ApplicableStartDate,
                ApplicableEndDate = k.ApplicableEndDate,
                ProgressType = k.ProgressType,
                AppraisalYear = k.AppraisalYear,
                DocumentEvidencePath = k.DocumentEvidencePath,
                SelfReviewSummary = k.SelfReviewSummary,
                CreatedBy = k.CreatedBy,
                CreatedAt = k.CreatedAt,
                ModifiedBy = k.ModifiedBy,
                ModifiedAt = k.ModifiedAt,

                KpiItems = k.EmployeeKpiitems.Select(i => new EmployeeKpiItemDto
                {
                    KpiItemId = i.KpiitemId,
                    KpiId = i.Kpiid,
                    KpiObjective = i.Kpiobjective,
                    Weightage = i.Weightage,
                    Target = i.Target,
                    TaskCompleted = i.TaskCompleted,
                    SelfRating = i.SelfRating,
                    Remarks = i.Remarks
                }).ToList()
            };
        }

    }
}
