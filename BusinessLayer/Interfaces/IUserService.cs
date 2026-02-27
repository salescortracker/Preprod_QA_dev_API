using BusinessLayer.Common;
using BusinessLayer.DTOs;
using DataAccessLayer.DBContext;

namespace BusinessLayer.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetAllUsersAsync(int userCompanyId);
        Task<IEnumerable<DataAccessLayer.DBContext.User>> GetcmpregAllUsersAsync(int CompanyId, int regionId);
        Task<User?> GetUserByIdAsync(int id);
        Task<User> CreateUserAsync(UserCreateDto userDto);
        Task<User?> UpdateUserAsync(UserCreateDto updatedUser);
        Task<bool> DeleteUserAsync(int id);
        Task<object?> VerifyLoginAsync(string username, string password);
        Task SendWelcomeEmailAsync(User user, string password);
        Task<ApiResponse<bool>> ChangePasswordAsync(PasswordChangeDto dto);

        Task<ApiResponse<bool>> SendOtpAsync(string email);
        Task<ApiResponse<bool>> VerifyOtpAsync(string email, string otp);
        Task<ApiResponse<bool>> ResetPasswordAsync(string email, string newPassword);
    }
}
