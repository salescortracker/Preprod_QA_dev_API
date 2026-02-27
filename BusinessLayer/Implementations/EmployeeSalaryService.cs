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
    public class EmployeeSalaryService: IEmployeeSalaryService
    {
        private readonly HRMSContext _context;

        public EmployeeSalaryService(HRMSContext context)
        {
            _context = context;
        }


        public async Task<List<EmployeeSalaryDto>> GetAllAssignedSalariesAsync(int userId)
        {
            var salaries = await _context.EmployeeSalaries
                .Where(x => x.UserId == userId)
                .Select(x => new EmployeeSalaryDto
                {
                    EmployeeSalaryId = x.EmployeeSalaryId,
                    EmployeeId = x.EmployeeId,
                    StructureId = x.StructureId,
                    EffectiveFrom = x.EffectiveFrom,
                    CTC = x.Ctc,
                    IsActive = x.IsActive,
                    CompanyId = x.CompanyId,
                    RegionId = x.RegionId,
                    UserId = x.UserId,
                    CreatedAt = x.CreatedAt
                })
                .ToListAsync();

            return salaries;
        }

        // 🔥 Assign Salary
        public async Task<EmployeeSalaryDto> AssignSalaryAsync(EmployeeSalaryDto dto, int userId)
        {
            // 1️⃣ Validate Structure belongs to user
            var structure = await _context.SalaryStructures
                .FirstOrDefaultAsync(x => x.StructureId == dto.StructureId && x.UserId == userId);

            if (structure == null)
                throw new Exception("Invalid Salary Structure");

            // 2️⃣ Deactivate existing active salary
            var existingActive = await _context.EmployeeSalaries
                .FirstOrDefaultAsync(x => x.EmployeeId == dto.EmployeeId && x.IsActive);

            if (existingActive != null)
            {
                existingActive.IsActive = false;
                existingActive.ModifiedAt = DateTime.UtcNow;
            }

            // 3️⃣ Insert new salary
            var employeeSalary = new EmployeeSalary
            {
                EmployeeId = dto.EmployeeId,
                StructureId = dto.StructureId,
                EffectiveFrom = dto.EffectiveFrom,
                Ctc = dto.CTC,
                IsActive = true,
                UserId = userId,
                CompanyId = structure.CompanyId,
                RegionId = structure.RegionId,
                CreatedAt = DateTime.UtcNow
            };

            _context.EmployeeSalaries.Add(employeeSalary);

            await _context.SaveChangesAsync();

            dto.EmployeeSalaryId = employeeSalary.EmployeeSalaryId;
            dto.IsActive = true;
            dto.UserId = userId;
            dto.CompanyId = employeeSalary.CompanyId;
            dto.RegionId = employeeSalary.RegionId;
            dto.CreatedAt = employeeSalary.CreatedAt;

            return dto;
        }

        // 🔥 Get Salary History
        public async Task<List<EmployeeSalaryDto>> GetEmployeeSalaryAsync(int employeeId, int userId)
        {
            return await _context.EmployeeSalaries
                .Where(x => x.EmployeeId == employeeId && x.UserId == userId)
                .OrderByDescending(x => x.EffectiveFrom)
                .Select(x => new EmployeeSalaryDto
                {
                    EmployeeSalaryId = x.EmployeeSalaryId,
                    EmployeeId = x.EmployeeId,
                    StructureId = x.StructureId,
                    EffectiveFrom = x.EffectiveFrom,
                    CTC = x.Ctc,
                    IsActive = x.IsActive,
                    CompanyId = x.CompanyId,
                    RegionId = x.RegionId,
                    UserId = x.UserId,
                    CreatedAt = x.CreatedAt,
                    ModifiedAt = x.ModifiedAt
                })
                .ToListAsync();
        }
    }
}
