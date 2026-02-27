using BusinessLayer.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Interfaces
{
    public interface ITimesheetService
    {
        Task<LoggedInUserDto> GetLoggedInUserAsync(int userId);
        Task<int> SaveTimesheetAsync(TimesheetRequestDto dto);
        Task<IEnumerable<TimesheetListDto>> GetMyTimesheetsAsync(int userId);
        Task<bool> SendSelectedTimesheetsAsync(List<int> timesheetIds);
        Task<IEnumerable<ManagerTimesheetDto>> GetTimesheetsForManagerAsync(int managerUserId);
        Task<ManagerTimesheetDto> GetTimesheetDetailAsync(int timesheetId);

        Task<bool> ApproveTimesheetsAsync(List<int> ids, string comments);
        Task<bool> RejectTimesheetsAsync(List<int> ids, string comments);
    }
}
