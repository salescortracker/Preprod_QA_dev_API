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
    public class CompanyPolicyService: ICompanyPolicyService
    {
        private readonly HRMSContext _context;

        private readonly string _uploadPath = null;
          //  @"C:\Users\vamsh\Documents\GitHub\QA_Latest_Dev_API\HRMS_Backend\wwwroot\Uploads\companypolicyfiles";

        public CompanyPolicyService(HRMSContext context)
        {
            _context = context;

            //if (!Directory.Exists(_uploadPath))
            //    Directory.CreateDirectory(_uploadPath);
        }

        // ---------------------------------------------------
        // GET ALL POLICIES
        // ---------------------------------------------------
        public async Task<List<CompanyPolicyDto>> GetAllPolicies()
        {
            return await _context.CompanyPolicies
                .Include(c => c.Category)
                .Select(x => new CompanyPolicyDto
                {
                    PolicyId = x.PolicyId,
                    CompanyId = x.CompanyId,
                    RegionId = x.RegionId,
                    Title = x.Title,
                    CategoryId = x.CategoryId,
                    CategoryName = x.Category.PolicyCategoryName,
                    EffectiveDate = x.EffectiveDate.ToDateTime(TimeOnly.MinValue),
                    Description = x.Description,
                    FileName = x.FileName,
                    FilePath = x.FilePath
                })
                .ToListAsync();
        }

        // ---------------------------------------------------
        // GET POLICY BY ID
        // ---------------------------------------------------
        public async Task<CompanyPolicyDto?> GetPolicyById(int id)
        {
            var p = await _context.CompanyPolicies
                .Include(c => c.Category)
                .FirstOrDefaultAsync(x => x.PolicyId == id);

            if (p == null)
                return null;

            return new CompanyPolicyDto
            {
                PolicyId = p.PolicyId,
                CompanyId = p.CompanyId,
                RegionId = p.RegionId,
                Title = p.Title,
                CategoryId = p.CategoryId,
                CategoryName = p.Category.PolicyCategoryName,
                EffectiveDate = p.EffectiveDate.ToDateTime(TimeOnly.MinValue),
                Description = p.Description,
                FileName = p.FileName,
                FilePath = p.FilePath
            };
        }

        // ---------------------------------------------------
        // CATEGORY DROPDOWN
        // ---------------------------------------------------
        public async Task<List<CreateUpdatePolicyCategoryDto>> GetPolicyCategoryDropdown(int companyId, int regionId)
        {
            return await _context.PolicyCategories
                .Where(x =>
                    x.CompanyId == companyId &&
                    x.RegionId == regionId &&
                    x.IsActive &&
                    !x.IsDeleted)
                .Select(x => new CreateUpdatePolicyCategoryDto
                {
                    PolicyCategoryId = x.PolicyCategoryId,
                    CompanyId = x.CompanyId,
                    RegionId = x.RegionId,
                    PolicyCategoryName = x.PolicyCategoryName,
                    Description = x.Description,
                    IsActive = x.IsActive,
                    UserId = x.UserId
                })
                .ToListAsync();
        }

        // ---------------------------------------------------
        // CREATE
        // ---------------------------------------------------
        public async Task<CompanyPolicyDto> CreatePolicy(CompanyPolicyDto dto)
        {
            string? fileName = null;
            string? filePath = null;

            if (dto.File != null)
            {
                fileName = $"{Guid.NewGuid()}_{dto.File.FileName}";
                var fullPath = Path.Combine(_uploadPath, fileName);

                using var stream = new FileStream(fullPath, FileMode.Create);
                await dto.File.CopyToAsync(stream);

                filePath = $"/Uploads/companypolicyfiles/{fileName}";
            }

            var entity = new CompanyPolicy
            {
                CompanyId = dto.CompanyId,
                RegionId = dto.RegionId,
                Title = dto.Title,
                CategoryId = dto.CategoryId,
                EffectiveDate = DateOnly.FromDateTime(dto.EffectiveDate),
                Description = dto.Description,
                FileName = fileName ?? "",
                FilePath = filePath,
                CreatedAt = DateTime.Now
            };

            _context.CompanyPolicies.Add(entity);
            await _context.SaveChangesAsync();

            dto.PolicyId = entity.PolicyId;
            dto.FileName = entity.FileName;
            dto.FilePath = entity.FilePath;

            return dto;
        }

        // ---------------------------------------------------
        // UPDATE
        // ---------------------------------------------------
        public async Task<CompanyPolicyDto?> UpdatePolicy(CompanyPolicyDto dto)
        {
            var entity = await _context.CompanyPolicies.FindAsync(dto.PolicyId);
            if (entity == null)
                return null;

            entity.CompanyId = dto.CompanyId;
            entity.RegionId = dto.RegionId;
            entity.Title = dto.Title;
            entity.CategoryId = dto.CategoryId;
            entity.Description = dto.Description;
            entity.EffectiveDate = DateOnly.FromDateTime(dto.EffectiveDate);
            entity.ModifiedAt = DateTime.Now;

            if (dto.File != null)
            {
                var newName = $"{Guid.NewGuid()}_{dto.File.FileName}";
                var fullPath = Path.Combine(_uploadPath, newName);

                using var stream = new FileStream(fullPath, FileMode.Create);
                await dto.File.CopyToAsync(stream);

                entity.FileName = newName;
                entity.FilePath = $"/Uploads/companypolicyfiles/{newName}";
            }

            await _context.SaveChangesAsync();
            return dto;
        }

        // ---------------------------------------------------
        // DELETE
        // ---------------------------------------------------
        public async Task<bool> DeletePolicy(int id)
        {
            var entity = await _context.CompanyPolicies.FindAsync(id);
            if (entity == null)
                return false;

            _context.CompanyPolicies.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
