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
    public interface IProjectStatusAdminService
    {
        Task<ApiResponse<IEnumerable<ProjectStatusDto>>> GetAllProject(int userId);

        Task<ApiResponse<ProjectStatus?>> GetByIdProjectAsync(int id);

        Task<ApiResponse<string>> CreateProjectAsync(ProjectStatusDto dto);

        Task<ApiResponse<string>> UpdateProjectAsync(ProjectStatusDto dto);

        Task<ApiResponse<string>> DeleteProjectAsync(int id);
    }
}
