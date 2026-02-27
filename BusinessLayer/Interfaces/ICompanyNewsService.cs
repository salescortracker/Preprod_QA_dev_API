using BusinessLayer.DTOs;
using DataAccessLayer.DBContext;

namespace BusinessLayer.Interfaces
{
    public interface ICompanyNewsService
    {
        Task<bool> AddCompanyNewsAsync(CompanyNewsDto dto, int createdByUserId);

        // Get all news (for listing)
        Task<List<CompanyNews>> GetAllCompanyNewsAsync();

        // Get departments (for category dropdown)
        Task<List<Department>> GetDepartmentsAsync();

        // Get a single news item by ID (optional, for edit)
        Task<CompanyNews> GetCompanyNewsByIdAsync(int newsId);

        // Update existing news
        Task<bool> UpdateCompanyNewsAsync(CompanyNewsDto dto, int updatedByUserId);

        // Delete news by ID
        Task<bool> DeleteCompanyNewsAsync(int newsId, int deletedByUserId);

        Task<List<CompanyNewsDto>> GetFilteredCompanyNewsAsync(string? category, DateTime? date);

    }
}
