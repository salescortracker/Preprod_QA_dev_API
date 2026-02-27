using BusinessLayer.DTOs;
using DataAccessLayer.DBContext;

namespace BusinessLayer.Interfaces
{
    public interface ICompanyService
    {
        Task<IEnumerable<CompanyDto>> GetAllCompaniesAsync(int userId);
        Task<CompanyDto?> GetCompanyByIdAsync(int id);
        Task<IEnumerable<CompanyDto>> SearchCompaniesAsync(object filter);
        Task<CompanyDto> AddCompanyAsync(CompanyDto dto);
        Task<IEnumerable<Company>> AddCompaniesAsync(List<CompanyDto> dtos);
        Task<CompanyDto> UpdateCompanyAsync(int id, CompanyDto dto);
        Task<bool> DeleteCompanyAsync(int id);
    }
}
