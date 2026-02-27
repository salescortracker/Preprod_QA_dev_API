using BusinessLayer.Common;
using BusinessLayer.DTOs;
using DataAccessLayer.DBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Interfaces
{
    public interface IGenderService
    {
        Task<IEnumerable<GenderDto>> GetAllGendersAsync();
        Task<ApiResponse<IEnumerable<GenderDto>>> GetAllAsync(int companyId, int regionId, int userId);
        Task<GenderDto?> GetGenderByIdAsync(int id);
        Task<IEnumerable<GenderDto>> SearchGenderAsync(object filter);
        Task<GenderDto> AddGenderAsync(GenderDto dto);
        Task<GenderDto> UpdateGenderAsync(GenderDto dto);
        Task<bool> DeleteGenderAsync(int id);
        Task<IEnumerable<Gender>> AddGendersAsync(List<GenderDto> dtos);
    }
}
