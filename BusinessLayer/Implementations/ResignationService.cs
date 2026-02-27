using BusinessLayer.DTOs;
using BusinessLayer.Interfaces;
using DataAccessLayer.DBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Implementations
{
    public class ResignationService: IResignationService
    {
        private readonly HRMSContext _db;

        public ResignationService(HRMSContext db)
        {
            _db = db;
        }

        public List<ResignationDto> GetAll(int companyId, int regionId)
        {
            return _db.Resignations
                .Where(x => x.CompanyId == companyId &&
                            x.RegionId == regionId &&
                            x.IsDeleted == false)
                .Select(x => new ResignationDto
                {
                    ResignationId = x.ResignationId,
                    CompanyId = x.CompanyId,
                    RegionId = x.RegionId,
                    ResignationType = x.ResignationType,
                    NoticePeriodDays = x.NoticePeriodDays,
                    IsActive = x.IsActive
                })
                .ToList();
        }

        public ResignationDto? GetById(int id)
        {
            return _db.Resignations
                .Where(x => x.ResignationId == id && x.IsDeleted == false)
                .Select(x => new ResignationDto
                {
                    ResignationId = x.ResignationId,
                    CompanyId = x.CompanyId,
                    RegionId = x.RegionId,
                    ResignationType = x.ResignationType,
                    NoticePeriodDays = x.NoticePeriodDays,
                    IsActive = x.IsActive
                })
                .FirstOrDefault();
        }

        public bool Create(ResignationDto dto, int userId)
        {
            var entity = new Resignation
            {
                CompanyId = dto.CompanyId,
                RegionId = dto.RegionId,
                ResignationType = dto.ResignationType,
                NoticePeriodDays = dto.NoticePeriodDays,
                IsActive = true,
                IsDeleted = false,
                CreatedBy = userId,
                CreatedAt = DateTime.Now,
                UserId = userId
            };

            _db.Resignations.Add(entity);
            _db.SaveChanges();
            return true;
        }

        public bool Update(int id, ResignationDto dto, int userId)
        {
            var entity = _db.Resignations.FirstOrDefault(x => x.ResignationId == id);
            if (entity == null) return false;

            entity.ResignationType = dto.ResignationType;
            entity.NoticePeriodDays = dto.NoticePeriodDays;
            entity.IsActive = dto.IsActive;

            entity.ModifiedBy = userId;
            entity.ModifiedAt = DateTime.Now;

            _db.SaveChanges();
            return true;
        }

        public bool Delete(int id, int userId)
        {
            var entity = _db.Resignations.FirstOrDefault(x => x.ResignationId == id);
            if (entity == null) return false;

            entity.IsDeleted = true;
            entity.ModifiedBy = userId;
            entity.ModifiedAt = DateTime.Now;

            _db.SaveChanges();
            return true;
        }
    
}
}
