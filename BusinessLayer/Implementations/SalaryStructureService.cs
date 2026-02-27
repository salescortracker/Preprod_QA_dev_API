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
    public class SalaryStructureService: ISalaryStructureService
    {
        private readonly HRMSContext _context;

        public SalaryStructureService(HRMSContext context)
        {
            _context = context;
        }

        public async Task<List<SalaryStructureDto>> GetAllSalaryStructuresAsync(int userId)
        {
            return await _context.SalaryStructures
                .Where(x => x.UserId == userId)
                .Select(x => new SalaryStructureDto
                {
                    StructureId = x.StructureId,
                    StructureName = x.StructureName,
                    DepartmentId = x.DepartmentId,
                    DesignationId = x.DesignationId,
                    GradeId = x.GradeId,
                    IsActive = x.IsActive,
                    CompanyId = x.CompanyId,
                    RegionId = x.RegionId,
                    UserId = x.UserId,
                    CreatedAt = x.CreatedAt,
                    ModifiedAt = x.ModifiedAt
                })
                .ToListAsync();
        }

        public async Task<SalaryStructureDto?> GetSalaryStructureByIdAsync(int id, int userId)
        {
            var structure = await _context.SalaryStructures
                .FirstOrDefaultAsync(x => x.StructureId == id && x.UserId == userId);

            if (structure == null) return null;

            var components = await _context.SalaryStructureComponents
                .Where(x => x.StructureId == id)
                .Select(x => new SalaryStructureComponentDto
                {
                    ComponentId = x.ComponentId,
                    Value = x.Value,
                    CalculationType = x.CalculationType
                })
                .ToListAsync();

            return new SalaryStructureDto
            {
                StructureId = structure.StructureId,
                StructureName = structure.StructureName,
                DepartmentId = structure.DepartmentId,
                DesignationId = structure.DesignationId,
                GradeId = structure.GradeId,
                IsActive = structure.IsActive,
                CompanyId = structure.CompanyId,
                RegionId = structure.RegionId,
                UserId = structure.UserId,
                CreatedAt = structure.CreatedAt,
                ModifiedAt = structure.ModifiedAt,
                Components = components
            };
        }

        public async Task<SalaryStructureDto> CreateSalaryStructureAsync(SalaryStructureDto dto, int userId)
        {
            var structure = new SalaryStructure
            {
                StructureName = dto.StructureName,
                DepartmentId = dto.DepartmentId,
                DesignationId = dto.DesignationId,
                GradeId = dto.GradeId,
                IsActive = true,
                UserId = userId,
                CompanyId = dto.CompanyId,
                RegionId = dto.RegionId,
                CreatedAt = DateTime.UtcNow
            };

            _context.SalaryStructures.Add(structure);
            await _context.SaveChangesAsync();

            if (dto.Components != null)
            {
                foreach (var comp in dto.Components)
                {
                    _context.SalaryStructureComponents.Add(new SalaryStructureComponent
                    {
                        StructureId = structure.StructureId,
                        ComponentId = comp.ComponentId,
                        Value = comp.Value,
                        CalculationType = comp.CalculationType
                    });
                }

                await _context.SaveChangesAsync();
            }

            dto.StructureId = structure.StructureId;
            return dto;
        }

        public async Task<SalaryStructureDto?> UpdateSalaryStructureAsync(int id, SalaryStructureDto dto, int userId)
        {
            var structure = await _context.SalaryStructures
                .FirstOrDefaultAsync(x => x.StructureId == id && x.UserId == userId);

            if (structure == null) return null;

            structure.StructureName = dto.StructureName;
            structure.DepartmentId = dto.DepartmentId;
            structure.DesignationId = dto.DesignationId;
            structure.GradeId = dto.GradeId;
            structure.IsActive = dto.IsActive;
            structure.CompanyId = dto.CompanyId;
            structure.RegionId = dto.RegionId;
            structure.ModifiedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            var oldComponents = _context.SalaryStructureComponents
                .Where(x => x.StructureId == id);

            _context.SalaryStructureComponents.RemoveRange(oldComponents);
            await _context.SaveChangesAsync();

            if (dto.Components != null)
            {
                foreach (var comp in dto.Components)
                {
                    _context.SalaryStructureComponents.Add(new SalaryStructureComponent
                    {
                        StructureId = id,
                        ComponentId = comp.ComponentId,
                        Value = comp.Value,
                        CalculationType = comp.CalculationType
                    });
                }

                await _context.SaveChangesAsync();
            }

            return dto;
        }

        public async Task<bool> DeleteSalaryStructureAsync(int id, int userId)
        {
            var structure = await _context.SalaryStructures
                .FirstOrDefaultAsync(x => x.StructureId == id && x.UserId == userId);

            if (structure == null) return false;

            var components = _context.SalaryStructureComponents
                .Where(x => x.StructureId == id);

            _context.SalaryStructureComponents.RemoveRange(components);
            _context.SalaryStructures.Remove(structure);

            await _context.SaveChangesAsync();
            return true;
        }
    }
}
