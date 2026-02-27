using BusinessLayer.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Interfaces
{
    public interface IEventService
    {
        Task<List<EventDTO>> GetAllAsync();
        Task<EventDTO?> GetByIdAsync(int id);
        Task<EventDTO> CreateAsync(EventDTO dto);
        Task<EventDTO?> UpdateAsync(EventDTO dto);
        Task<bool> DeleteAsync(int id);
    }
}
