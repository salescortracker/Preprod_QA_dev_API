using BusinessLayer.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Interfaces
{
    public interface IRegionService
    {
        Task<IEnumerable<RegionDto>> GetAllRegionsAsync(int userId);
        Task<RegionDto?> GetRegionByIdAsync(int id);
        Task<IEnumerable<RegionDto>> SearchRegionsAsync(object filter);
        Task<RegionDto> AddRegionAsync(object model);
        Task<RegionDto> UpdateRegionAsync(int id, object model);
        Task<bool> DeleteRegionAsync(int id);
    }
}
