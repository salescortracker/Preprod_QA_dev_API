using BusinessLayer.DTOs;
using BusinessLayer.Interfaces;
using DataAccessLayer.DBContext;
using Microsoft.EntityFrameworkCore;


namespace BusinessLayer.Implementations
{
   
    public class adminService:IadminService
    {
        private readonly HRMSContext _context;
        public adminService(HRMSContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Relationship>> GetAllrelatiopnshipByUserAsync(int companyId, int regionId)
        {
            return await _context.Relationships
                .Where(x => !x.IsDeleted &&
                           
                            x.CompanyId == companyId && x.RegionId==regionId)
                           // x.RegionId == regionId)
                .ToListAsync();
        }
        public async Task<IEnumerable<Relationship>> GetAllUserIdrelatiopnshipByUserAsync(int usercompanyId)
        {
            return await _context.Relationships
                .Where(x => !x.IsDeleted &&

                            x.UserId == usercompanyId)
                // x.RegionId == regionId)
                .ToListAsync();
        }

        public async Task<RelationshipDto> AddrelatiopnshipAsync(RelationshipDto model)
        {
            //entity. = DateTime.UtcNow;
            var entity = new Relationship
            {

                RelationshipName=model.RelationshipName,
                RegionId=model.regionId,
                UserId=model.userId,
                CompanyId=model.companyId,
                IsActive=model.isActive,
            };
            await _context.Relationships.AddAsync(entity);
            await _context.SaveChangesAsync();
            return MapToDto(entity);
        }

        public async Task<RelationshipDto?> UpdaterelatiopnshipAsync(RelationshipDto entity)
        {
            var dbObj = await _context.Relationships
                    .FirstOrDefaultAsync(x => x.RelationshipId == entity.RelationshipID && !x.IsDeleted);

            if (dbObj == null)
                return null;

            dbObj.RelationshipName = entity.RelationshipName;
         //   dbObj.Description = entity.Description;
            dbObj.IsActive = entity.isActive;
           // dbObj.ModifiedBy = entity.ModifiedBy;
            dbObj.ModifiedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return MapToDto(dbObj);
        }


        public async Task<bool> Deleterelatiopnship(int relationshipId)
        {
            var dbObj = await _context.Relationships
                        .FirstOrDefaultAsync(x => x.RelationshipId == relationshipId);

            if (dbObj == null)
                return false;

            dbObj.IsDeleted = true;
           
            dbObj.ModifiedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }
        private static RelationshipDto MapToDto(Relationship entity)
        {
            return new RelationshipDto
            {
                RelationshipID = entity.RelationshipId,
                regionId = entity.RegionId,
                companyId = entity.CompanyId,
                RelationshipName = entity.RelationshipName,
                isActive = entity.IsActive

            };
        }
        #region gender Details

        public async Task<IEnumerable<Gender>> GetAllgenderByUserAsync(int companyId, int regionId)
        {
            return await _context.Genders
                .Where(x => !x.IsDeleted &&
                           
                            x.CompanyId == companyId &&
                            x.RegionId == regionId)
                .ToListAsync();
        }

        public async Task<Gender> AddgenderAsync(Gender entity)
        {
            entity.CreatedAt = DateTime.UtcNow;
            await _context.Genders.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<Gender?> UpdategenderAsync(Gender entity)
        {
            var dbObj = await _context.Genders
                .FirstOrDefaultAsync(x => x.GenderId == entity.GenderId && !x.IsDeleted);

            if (dbObj == null)
                return null;

            dbObj.GenderName = entity.GenderName;
            dbObj.Description = entity.Description;
            dbObj.IsActive = entity.IsActive;
            dbObj.ModifiedBy = entity.ModifiedBy;
            dbObj.ModifiedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return dbObj;
        }

        public async Task<bool> DeletegenderAsync(int genderId)
        {
            var dbObj = await _context.Genders
                .FirstOrDefaultAsync(x => x.GenderId == genderId && !x.IsDeleted);

            if (dbObj == null)
                return false;

            dbObj.IsDeleted = true;
           
            dbObj.ModifiedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }
        #endregion

    }
}
