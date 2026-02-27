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
    public class EventService: IEventService
    {
        private readonly HRMSContext _context;

        public EventService(HRMSContext context)
        {
            _context = context;
        }

        public async Task<List<EventDTO>> GetAllAsync()
        {
            return await _context.Events
                .Include(e => e.EventType)
                .Select(e => new EventDTO
                {
                    EventId = e.EventId,
                    CompanyId = e.CompanyId,
                    RegionId = e.RegionId,
                    UserId = e.UserId,
                    EventTypeId = e.EventTypeId,
                    EventTypeName = e.EventType.EventTypeName,
                    EventName = e.EventName,
                    EventDate = e.EventDate,
                    Description = e.Description,
                    IsActive = e.IsActive,
                    CreatedBy = e.CreatedBy,
                    CreatedDate = e.CreatedDate,
                    ModifiedBy = e.ModifiedBy,
                    ModifiedDate = e.ModifiedDate,
                    RoleId = e.RoleId
                }).ToListAsync();
        }

        public async Task<EventDTO?> GetByIdAsync(int id)
        {
            var e = await _context.Events
                .Include(x => x.EventType)
                .FirstOrDefaultAsync(x => x.EventId == id);

            if (e == null) return null;

            return new EventDTO
            {
                EventId = e.EventId,
                CompanyId = e.CompanyId,
                RegionId = e.RegionId,
                UserId = e.UserId,
                EventTypeId = e.EventTypeId,
                EventTypeName = e.EventType.EventTypeName,
                EventName = e.EventName,
                EventDate = e.EventDate,
                Description = e.Description,
                IsActive = e.IsActive,
                CreatedBy = e.CreatedBy,
                CreatedDate = e.CreatedDate,
                ModifiedBy = e.ModifiedBy,
                ModifiedDate = e.ModifiedDate,
                RoleId = e.RoleId
            };
        }

        public async Task<EventDTO> CreateAsync(EventDTO dto)
        {
            var entity = new Event
            {
                CompanyId = dto.CompanyId,
                RegionId = dto.RegionId,
                UserId = dto.UserId,
                EventTypeId = dto.EventTypeId,
                EventName = dto.EventName,
                EventDate = dto.EventDate,
                Description = dto.Description,
                IsActive = true,
                CreatedBy = dto.CreatedBy,
                CreatedDate = DateTime.Now,
                RoleId = dto.RoleId
            };

            _context.Events.Add(entity);
            await _context.SaveChangesAsync();

            dto.EventId = entity.EventId;
            return dto;
        }

        public async Task<EventDTO?> UpdateAsync(EventDTO dto)
        {
            var entity = await _context.Events.FindAsync(dto.EventId);
            if (entity == null) return null;

            entity.CompanyId = dto.CompanyId;
            entity.RegionId = dto.RegionId;
            entity.UserId = dto.UserId;
            entity.EventTypeId = dto.EventTypeId;
            entity.EventName = dto.EventName;
            entity.EventDate = dto.EventDate;
            entity.Description = dto.Description;
            entity.IsActive = dto.IsActive;
            entity.ModifiedBy = dto.ModifiedBy;
            entity.ModifiedDate = DateTime.Now;
            entity.RoleId = dto.RoleId;

            await _context.SaveChangesAsync();
            return dto;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _context.Events.FindAsync(id);
            if (entity == null) return false;

            _context.Events.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
