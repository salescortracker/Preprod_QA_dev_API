using BusinessLayer.DTOs;
using BusinessLayer.Interfaces;
using DataAccessLayer.DBContext;
using DataAccessLayer.Repositories.GeneralRepository;

namespace BusinessLayer.Implementations
{
    public class CompanyService : ICompanyService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CompanyService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<CompanyDto>> GetAllCompaniesAsync(int userId)
        {
            var companies = await _unitOfWork.Repository<Company>().GetAllAsync();

            return companies
                .Where(x => x.UserId == userId)
                .OrderByDescending(x => x.CompanyId)
                .Select(MapToDto);
        }

        public async Task<CompanyDto?> GetCompanyByIdAsync(int id)
        {
            var company = await _unitOfWork.Repository<Company>().GetByIdAsync(id);
            return company == null ? null : MapToDto(company);
        }

        public async Task<IEnumerable<CompanyDto>> SearchCompaniesAsync(object filter)
        {
            var props = filter.GetType().GetProperties();
            var allCompanies = await _unitOfWork.Repository<Company>().GetAllAsync();
            var query = allCompanies.AsQueryable();

            foreach (var prop in props)
            {
                var name = prop.Name;
                var value = prop.GetValue(filter);

                if (value == null) continue;

                switch (name)
                {
                    case nameof(Company.CompanyName):
                        query = query.Where(c =>
                            c.CompanyName != null &&
                            c.CompanyName.Contains(value.ToString()!));
                        break;

                    case nameof(Company.CompanyCode):
                        query = query.Where(c =>
                            c.CompanyCode == value.ToString());
                        break;

                    case nameof(Company.IsActive):
                        bool isActive = Convert.ToBoolean(value);
                        query = query.Where(c => c.IsActive == isActive);
                        break;
                }
            }

            return query.Select(MapToDto);
        }

        public async Task<CompanyDto> AddCompanyAsync(CompanyDto dto)
        {
            var entity = new Company
            {
                CompanyName = dto.companyName,
                CompanyCode = dto.companyCode,
                IndustryType = dto.industryType,
                Headquarters = dto.headquarters,
                IsActive = dto.isActive,
                UserId = dto.userId,
                CreatedDate = DateTime.UtcNow
            };

            await _unitOfWork.Repository<Company>().AddAsync(entity);
            await _unitOfWork.CompleteAsync();

            return MapToDto(entity);
        }

        public async Task<IEnumerable<Company>> AddCompaniesAsync(List<CompanyDto> dtos)
        {
            var companies = dtos.Select(dto => new Company
            {
                CompanyName = dto.companyName,
                CompanyCode = dto.companyCode,
                IndustryType = dto.industryType,
                Headquarters = dto.headquarters,
                IsActive = dto.isActive,
                UserId = dto.userId,
                CreatedDate = DateTime.UtcNow
            }).ToList();

            await _unitOfWork.Repository<Company>().AddRangeAsync(companies);
            await _unitOfWork.CompleteAsync();

            return companies;
        }

        public async Task<CompanyDto> UpdateCompanyAsync(int id, CompanyDto dto)
        {
            var entity = await _unitOfWork.Repository<Company>().GetByIdAsync(id);

            if (entity == null)
                throw new Exception("Company not found");

            entity.CompanyName = dto.companyName;
            entity.CompanyCode = dto.companyCode;
            entity.IndustryType = dto.industryType;
            entity.Headquarters = dto.headquarters;
            entity.IsActive = dto.isActive;
            entity.UserId = dto.userId;
            entity.ModifiedAt = DateTime.UtcNow;

            _unitOfWork.Repository<Company>().Update(entity);
            await _unitOfWork.CompleteAsync();

            return MapToDto(entity);
        }

        public async Task<bool> DeleteCompanyAsync(int id)
        {
            try
            {
                var entity = await _unitOfWork.Repository<Company>().GetByIdAsync(id);

                if (entity == null) return false;

                _unitOfWork.Repository<Company>().Remove(entity);
                await _unitOfWork.CompleteAsync();

                return true;
            }
            catch
            {
                return false;
            }
        }

        private CompanyDto MapToDto(Company c)
        {
            return new CompanyDto
            {
                CompanyId = c.CompanyId,
                companyName = c.CompanyName,
                companyCode = c.CompanyCode,
                industryType = c.IndustryType,
                headquarters = c.Headquarters,
                isActive = c.IsActive,
                userId = c.UserId
            };
        }
    }
}
