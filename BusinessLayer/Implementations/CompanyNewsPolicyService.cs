using BusinessLayer.DTOs;
using BusinessLayer.Interfaces.BusinessLayer.Interfaces;
using DataAccessLayer.DBContext;
using DataAccessLayer.Repositories.GeneralRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLayer.Interfaces;

namespace BusinessLayer.Implementations
{
    public class CompanyNewsPolicyService : ICompanyNewsPolicyService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CompanyNewsPolicyService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // =========================================================
        // ====================== COMPANY NEWS =====================
        // =========================================================

        /// <summary>
        /// Get all news based on UserId
        /// </summary>
        public async Task<IEnumerable<CompanyNewsMasterDto>> GetAllNewsAsync(int userId)
        {
            var news = await _unitOfWork.Repository<CompanyNewsMaster>().GetAllAsync();

            return news
                .Where(x => x.UserId == userId)
                .OrderByDescending(x => x.NewsId)
                .Select(MapNewsToDto)
                .ToList();
        }

        /// <summary>
        /// Get only today's news based on PostedDate and UserId
        /// </summary>
        public async Task<IEnumerable<CompanyNewsMasterDto>> GetTodayNewsAsync(int userId)
        {
            var users = await _unitOfWork.Repository<User>().GetAllAsync();

            var user = users.FirstOrDefault(x => x.UserId == userId);

            if (user == null)
                return new List<CompanyNewsMasterDto>();

            var departmentId = user.DepartmentId;

            var today = DateOnly.FromDateTime(DateTime.Now);

            var news = await _unitOfWork.Repository<CompanyNewsMaster>().GetAllAsync();

            return news
                .Where(x =>
                    x.DepartmentId == departmentId &&
                    x.IsActive == true &&
                    x.PostedDate.HasValue &&
                    x.PostedDate.Value == today)
                .Select(MapNewsToDto)
                .ToList();
        }

        /// <summary>
        /// Get news by Id & UserId
        /// </summary>
        public async Task<CompanyNewsMasterDto?> GetNewsByIdAsync(int id, int userId)
        {
            var entity = await _unitOfWork.Repository<CompanyNewsMaster>().GetByIdAsync(id);

            if (entity == null || entity.UserId != userId)
                return null;

            return MapNewsToDto(entity);
        }

        /// <summary>
        /// Add new news
        /// </summary>
        public async Task<CompanyNewsMasterDto> AddNewsAsync(CompanyNewsMasterDto dto)
        {
            var entity = new CompanyNewsMaster
            {
                Title = dto.Title,
                Description = dto.Description,
                PostedDate = dto.PostedDate,
                ExpiryDate = dto.ExpiryDate,
                DepartmentId = dto.departmentId,
                Category = dto.Category,
                IsActive = dto.IsActive,
                UserId = dto.UserId,
                CompanyId = dto.CompanyId,
                RegionId = dto.RegionId,
                CreatedBy = dto.CreatedBy,
                CreatedAt = DateTime.UtcNow
            };

            await _unitOfWork.Repository<CompanyNewsMaster>().AddAsync(entity);
            await _unitOfWork.CompleteAsync();

            return MapNewsToDto(entity);
        }

        /// <summary>
        /// Update existing news
        /// </summary>
        public async Task<CompanyNewsMasterDto> UpdateNewsAsync(int id, CompanyNewsMasterDto dto)
        {
            var entity = await _unitOfWork.Repository<CompanyNewsMaster>().GetByIdAsync(id);

            if (entity == null)
                throw new Exception("News not found");

            entity.Title = dto.Title;
            entity.Description = dto.Description;
            entity.PostedDate = dto.PostedDate;
            entity.Category =   dto.Category;
            entity.DepartmentId = dto.departmentId;
            entity.ExpiryDate = dto.ExpiryDate;
            entity.IsActive = dto.IsActive;
            entity.UpdatedBy = dto.UpdatedBy;
            entity.UpdatedAt = DateTime.UtcNow;

            _unitOfWork.Repository<CompanyNewsMaster>().Update(entity);
            await _unitOfWork.CompleteAsync();

            return MapNewsToDto(entity);
        }

        /// <summary>
        /// Delete news based on UserId
        /// </summary>
        public async Task<bool> DeleteNewsAsync(int id, int userId)
        {
            var entity = await _unitOfWork.Repository<CompanyNewsMaster>().GetByIdAsync(id);

            if (entity == null || entity.UserId != userId)
                return false;

            _unitOfWork.Repository<CompanyNewsMaster>().Remove(entity);
            await _unitOfWork.CompleteAsync();

            return true;
        }


        // =========================================================
        // ====================== COMPANY POLICY ===================
        // =========================================================

        public async Task<IEnumerable<CompanyPolicyMasterDto>> GetAllPoliciesAsync(int userId)
        {
            var policies = await _unitOfWork.Repository<CompanyPoliciesMaster>().GetAllAsync();

            return policies
                .Where(x => x.UserId == userId)
                .OrderByDescending(x => x.PolicyId)
                .Select(MapPolicyToDto);
        }

        public async Task<IEnumerable<CompanyPolicyMasterDto>> GetTodayPoliciesAsync(int userId)
        {
            // Get all users
            var users = await _unitOfWork.Repository<User>().GetAllAsync();

            var user = users.FirstOrDefault(x => x.UserId == userId);

            if (user == null)
                return new List<CompanyPolicyMasterDto>();

            // Get DepartmentId from user
            var departmentId = user.DepartmentId;

            // Get today's date
            var today = DateOnly.FromDateTime(DateTime.Now);

            // Get all policies
            var policies = await _unitOfWork.Repository<CompanyPoliciesMaster>().GetAllAsync();

            // Filter policies
            return policies
                .Where(x =>
                    x.DepartmentId == departmentId &&
                    x.IsActive == true &&
                    x.PostedDate.HasValue &&
                    x.PostedDate.Value == today)
                .Select(MapPolicyToDto)
                .ToList();
        }

        public async Task<CompanyPolicyMasterDto?> GetPolicyByIdAsync(int id, int userId)
        {
            var entity = await _unitOfWork.Repository<CompanyPoliciesMaster>().GetByIdAsync(id);

            if (entity == null || entity.UserId != userId)
                return null;

            return MapPolicyToDto(entity);
        }

        public async Task<CompanyPolicyMasterDto> AddPolicyAsync(CompanyPolicyMasterDto dto)
        {
            var entity = new CompanyPoliciesMaster
            {
                PolicyTitle = dto.PolicyTitle,
                PolicyDescription = dto.PolicyDescription,
                PostedDate = dto.PostedDate,
                EffectiveDate = dto.EffectiveDate,
                ExpiryDate = dto.ExpiryDate,
                DepartmentId = dto.DepartmentId,
                AttachmentName = dto.AttachmentName,
                AttachmentPath  = dto.AttachmentPath,
                Category = dto.Category,
                IsActive = dto.IsActive,
                UserId = dto.UserId,
                CompanyId = dto.CompanyId,
                RegionId = dto.RegionId,
                CreatedBy = dto.CreatedBy,
                CreatedAt = DateTime.UtcNow
            };

            await _unitOfWork.Repository<CompanyPoliciesMaster>().AddAsync(entity);
            await _unitOfWork.CompleteAsync();

            return MapPolicyToDto(entity);
        }

        public async Task<CompanyPolicyMasterDto> UpdatePolicyAsync(int id, CompanyPolicyMasterDto dto)
        {
            var entity = await _unitOfWork.Repository<CompanyPoliciesMaster>().GetByIdAsync(id);

            if (entity == null)
                throw new Exception("Policy not found");

            entity.PolicyTitle = dto.PolicyTitle;
            entity.PolicyDescription = dto.PolicyDescription;
            entity.PostedDate = dto.PostedDate;
            entity.EffectiveDate = dto.EffectiveDate;
            entity.AttachmentPath = dto.AttachmentPath;
            entity.Category = dto.Category;
            entity.AttachmentName = dto.AttachmentName;
            entity.DepartmentId = dto.DepartmentId;
            entity.ExpiryDate = dto.ExpiryDate;
            entity.IsActive = dto.IsActive;
            entity.UpdatedBy = dto.UpdatedBy;
            entity.UpdatedAt = DateTime.UtcNow;

            _unitOfWork.Repository<CompanyPoliciesMaster>().Update(entity);
            await _unitOfWork.CompleteAsync();

            return MapPolicyToDto(entity);
        }

        public async Task<bool> DeletePolicyAsync(int id, int userId)
        {
            var entity = await _unitOfWork.Repository<CompanyPoliciesMaster>().GetByIdAsync(id);

            if (entity == null || entity.UserId != userId)
                return false;

            _unitOfWork.Repository<CompanyPoliciesMaster>().Remove(entity);
            await _unitOfWork.CompleteAsync();

            return true;
        }

        // ================= MAPPERS =================

        private CompanyNewsMasterDto MapNewsToDto(CompanyNewsMaster x)
        {
            return new CompanyNewsMasterDto
            {
                NewsId = x.NewsId,
                Title = x.Title,
                Description = x.Description,
                PostedDate = x.PostedDate,
                ExpiryDate = x.ExpiryDate,
                Category = x.Category,
                departmentId = x.DepartmentId,
                IsActive = x.IsActive,
                UserId = x.UserId,
                CompanyId = x.CompanyId,
                RegionId = x.RegionId,
                CreatedBy = x.CreatedBy,
                UpdatedBy = x.UpdatedBy,
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt
            };
        }

        private CompanyPolicyMasterDto MapPolicyToDto(CompanyPoliciesMaster x)
        {
            return new CompanyPolicyMasterDto
            {
                PolicyId = x.PolicyId,
                PolicyTitle = x.PolicyTitle,
                PolicyDescription = x.PolicyDescription,
                PostedDate = x.PostedDate,
                EffectiveDate = x.EffectiveDate,
                ExpiryDate = x.ExpiryDate,
                IsActive = x.IsActive,
                DepartmentId = x.DepartmentId,
                Category = x.Category,
                UserId = x.UserId,
                CompanyId = x.CompanyId,
                RegionId = x.RegionId,
                CreatedBy = x.CreatedBy,
                UpdatedBy = x.UpdatedBy,
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt,
                AttachmentName = x.AttachmentName,
                AttachmentPath = x.AttachmentPath,
            };
        }
    }
}
