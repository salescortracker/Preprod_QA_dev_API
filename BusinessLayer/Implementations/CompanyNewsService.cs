using BusinessLayer.DTOs;
using BusinessLayer.Interfaces;
using DataAccessLayer.DBContext;
using Microsoft.EntityFrameworkCore;

namespace BusinessLayer.Implementations
{
    public class CompanyNewsService: ICompanyNewsService
    {
        private readonly HRMSContext _context;

        public CompanyNewsService(HRMSContext context)
        {
            _context = context;
        }

        // ------------------------------------------------------
        // 1. Get DEPARTMENTS dropdown
        // ------------------------------------------------------
        public async Task<List<Department>> GetDepartmentsAsync()
        {
            return await _context.Departments
                .Where(d => d.IsActive && !d.IsDeleted)
                .ToListAsync();
        }

        // ------------------------------------------------------
        // 2. ADD NEWS (ONLY ADMIN CAN ADD)
        // ------------------------------------------------------
        public async Task<bool> AddCompanyNewsAsync(CompanyNewsDto dto, int createdByUserId)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(x => x.UserId == createdByUserId);

            if (user == null)
                return false;

            var role = await _context.RoleMasters
                .FirstOrDefaultAsync(r => r.RoleId == user.RoleId);

            if (role == null || role.RoleName?.ToLower() != "admin")
                return false;

            var news = new CompanyNews
            {
                Title = dto.Title,
                Description = dto.Description,
                Category = dto.Category,
                FromDate = dto.FromDate,
                ToDate = dto.ToDate,
                CompanyId = dto.CompanyId,
                RegionId = dto.RegionId,
                AttachmentName = dto.AttachmentName,
                AttachmentPath = dto.AttachmentPath,
                CreatedBy = createdByUserId,
                CreatedAt = DateTime.Now,
                IsActive = true,
                UserId = createdByUserId
            };

            _context.CompanyNews.Add(news);
            await _context.SaveChangesAsync();

            return true;
        }

        // ------------------------------------------------------
        // 3. ANY EMPLOYEE CAN SEE FILTERED NEWS (NEW METHOD)
        // ------------------------------------------------------
        public async Task<List<CompanyNewsDto>> GetFilteredCompanyNewsAsync(string? category, DateTime? date)
        {
            var query = _context.CompanyNews
                .Where(n => n.IsActive)
                .AsQueryable();

            if (!string.IsNullOrEmpty(category))
                query = query.Where(n => n.Category == category);

            if (date.HasValue)
                query = query.Where(n => n.FromDate <= DateOnly.FromDateTime(date.Value)
                                         && n.ToDate >= DateOnly.FromDateTime(date.Value));

            return await query
                .OrderByDescending(n => n.CreatedAt)
                .Select(n => new CompanyNewsDto
                {
                    NewsId = n.NewsId,
                    Title = n.Title,
                    Category = n.Category,
                    Description = n.Description,
                    FromDate = n.FromDate,
                    ToDate = n.ToDate,
                    CompanyId = n.CompanyId,
                    RegionId = n.RegionId,
                    AttachmentName = n.AttachmentName,
                    AttachmentPath = n.AttachmentPath
                })
                .ToListAsync();
        }

        // ------------------------------------------------------
        // 4. Get ALL NEWS (OLD METHOD FOR BACKWARD COMPATIBILITY)
        // ------------------------------------------------------
        public async Task<List<CompanyNews>> GetAllCompanyNewsAsync()
        {
            return await _context.CompanyNews
                .Where(n => n.IsActive)
                .OrderByDescending(n => n.CreatedAt)
                .ToListAsync();
        }

        // ------------------------------------------------------
        // 5. Update news (ADMIN ONLY)
        // ------------------------------------------------------
        public async Task<bool> UpdateCompanyNewsAsync(CompanyNewsDto dto, int updatedByUserId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.UserId == updatedByUserId);
            var role = await _context.RoleMasters.FirstOrDefaultAsync(r => r.RoleId == user.RoleId);
            if (user == null || role?.RoleName?.ToLower() != "admin") return false;

            var news = await _context.CompanyNews.FirstOrDefaultAsync(n => n.NewsId == dto.NewsId);
            if (news == null) return false;

            news.Title = dto.Title;
            news.Category = dto.Category;
            news.Description = dto.Description;
            news.FromDate = dto.FromDate;
            news.ToDate = dto.ToDate;
            news.AttachmentName = dto.AttachmentName;
            news.AttachmentPath = dto.AttachmentPath;
            news.UpdatedBy = updatedByUserId;
            news.UpdatedAt = DateTime.Now;

            await _context.SaveChangesAsync();
            return true;
        }

        // ------------------------------------------------------
        // 6. Delete news (ADMIN ONLY)
        // ------------------------------------------------------
        public async Task<bool> DeleteCompanyNewsAsync(int newsId, int userId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.UserId == userId);
            var role = await _context.RoleMasters.FirstOrDefaultAsync(r => r.RoleId == user.RoleId);
            if (user == null || role?.RoleName?.ToLower() != "admin") return false;

            var news = await _context.CompanyNews.FirstOrDefaultAsync(n => n.NewsId == newsId);
            if (news == null) return false;

            _context.CompanyNews.Remove(news);
            await _context.SaveChangesAsync();
            return true;
        }

        // ------------------------------------------------------
        // 7. Get single news by ID (for edit)
        // ------------------------------------------------------
        public async Task<CompanyNews> GetCompanyNewsByIdAsync(int newsId)
        {
            return await _context.CompanyNews
                .FirstOrDefaultAsync(n => n.NewsId == newsId && n.IsActive);
        }

    }
}
