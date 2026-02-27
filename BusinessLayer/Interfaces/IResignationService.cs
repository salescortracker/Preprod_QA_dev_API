using BusinessLayer.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Interfaces
{
    public interface IResignationService
    {
        List<ResignationDto> GetAll(int companyId, int regionId);
        ResignationDto? GetById(int id);
        bool Create(ResignationDto dto, int userId);
        bool Update(int id, ResignationDto dto, int userId);
        bool Delete(int id, int userId);
    }
}
