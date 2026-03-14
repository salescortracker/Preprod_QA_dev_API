using BusinessLayer.Common;
using BusinessLayer.DTOs;
using BusinessLayer.Interfaces;
using DataAccessLayer.DBContext;
using DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;


namespace BusinessLayer.Implementations
{
    public class UserService : IUserService
    {
        private readonly DataAccessLayer.DBContext.HRMSContext _context;
        private readonly IConfiguration _configuration;

        public UserService(DataAccessLayer.DBContext.HRMSContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<IEnumerable<DataAccessLayer.DBContext.User>> GetAllUsersAsync(int userCompanyId)
        {
            return await _context.Users.Where(x=>x.UserCompanyId==userCompanyId).ToListAsync();
        }
        public async Task<IEnumerable<DataAccessLayer.DBContext.User>> GetcmpregAllUsersAsync(int CompanyId,int regionId)
        {
            return await _context.Users.Where(x => x.CompanyId == CompanyId && x.RegionId==regionId).ToListAsync();
        }

        public async Task<DataAccessLayer.DBContext.User?> GetUserByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<DataAccessLayer.DBContext.User> CreateUserAsync(UserCreateDto userDto)
        {
            try
            {
                if (userDto == null)
                    throw new ArgumentNullException(nameof(userDto));

                // ✅ Auto-generate Employee Code if not provided
                string newEmployeeCode = userDto.EmployeeCode ?? await GenerateNextEmployeeCodeAsync();

                // ✅ Hash Password
                string hashedPassword = HashPassword(userDto.Password);

                // ✅ Create User Entity
                var user = new DataAccessLayer.DBContext.User
                {
                    CompanyId = userDto.CompanyID,
                    RegionId = userDto.RegionID,
                    EmployeeCode = newEmployeeCode,
                    FullName = userDto.FullName,
                    Email = userDto.Email,
                    PasswordHash = userDto.Password,
                    ReportingTo= userDto.reportingTo,
                    DepartmentId= userDto.departmentId,
                    RoleId = userDto.RoleId,
                    Status = "Active",
                    CreatedDate = DateTime.UtcNow,
                    UserCompanyId=userDto.UserCompanyId
                };

                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                // ✅ Send Welcome Email
                await SendWelcomeEmailAsync(
                   user,userDto.Password
                );


                return user;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<object?> VerifyLoginAsync(string username, string password)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
                    throw new ArgumentException("Username or password cannot be empty.");

                var userData = await (
    from u in _context.Users
    join r in _context.RoleMasters on u.RoleId equals r.RoleId
    join reg in _context.Regions on u.RegionId equals reg.RegionId
    join c in _context.Companies on u.CompanyId equals c.CompanyId
    join d in _context.Departments on u.DepartmentId equals d.DepartmentId into deptJoin
    from d in deptJoin.DefaultIfEmpty()
    join des in _context.Designations
        on u.DepartmentId equals des.DesignationId into desJoin
    from des in desJoin.DefaultIfEmpty()


    join rm in _context.Users on u.ReportingTo equals rm.UserId into managerJoin
    from rm in managerJoin.DefaultIfEmpty()

    where u.Email == username && u.PasswordHash == password

    select new
    {
        u.UserId,
        u.Email,
        u.FullName,

        RoleName = r.RoleName,
        RegionName = reg.RegionName,
        CompanyName = c.CompanyName,

        roleId = u.RoleId,
        companyId = u.CompanyId,
        regionId = u.RegionId,
        employeeCode = u.EmployeeCode,

        DepartmentId = u.DepartmentId,
        DepartmentName = d.DepartmentName, // 🔥 STRING

        ReportingManagerId = u.ReportingTo,
        ReportingManagerName = rm.FullName, // 🔥 STRING

        DesignationId = u.Designation,
        designation = u.Designation, // if stored as string in Users table

        personalEmail = u.Email,
        userLoginStatus = u.Userloginstatus,
        paswordChanged = u.Passwordchanged
    })
.FirstOrDefaultAsync();



                var loginStatus = await _context.Users.FirstOrDefaultAsync(u => u.Email == username);
                loginStatus.Userloginstatus = true;
                _context.Users.Update(loginStatus);
                await _context.SaveChangesAsync();

                return userData;
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Input error: {ex.Message}");
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error verifying login: {ex.Message}");
                return null;
            }
        }
        public async Task<ApiResponse<bool>> ChangePasswordAsync(PasswordChangeDto dto)
        {
            var user = await _context.Users                
                .FirstOrDefaultAsync(u => u.UserId == dto.UserId);

            if (user == null)
                return new ApiResponse<bool>(false, "User not found", false);

            // Old password check only if NOT first login
            if (!user.Userloginstatus.Value)
            {
                string oldHash = HashPassword(dto.OldPassword);
                if (user.PasswordHash != oldHash)
                    return new ApiResponse<bool>(false, "Old password is incorrect", false);
            }

            user.PasswordHash = dto.NewPassword;
            user.Userloginstatus =true;
            user.Passwordchanged = true;
             _context.SaveChangesAsync();

            await SendPasswordChangedEmailAsync(user, user.PasswordHash);
            return new ApiResponse<bool>(true, "Password updated successfully", true);
        }



        // 🔹 Generate Employee Code (Emp0001, Emp0002, etc.)
        private async Task<string> GenerateNextEmployeeCodeAsync()
        {
            var lastUser = await _context.Users
                .OrderByDescending(u => u.UserId)
                .FirstOrDefaultAsync();

            int nextNumber = 1;

            if (lastUser != null && !string.IsNullOrEmpty(lastUser.EmployeeCode))
            {
                string numberPart = new string(lastUser.EmployeeCode.SkipWhile(c => !char.IsDigit(c)).ToArray());
                if (int.TryParse(numberPart, out int lastNumber))
                    nextNumber = lastNumber + 1;
            }

            return $"EMP{nextNumber:D4}";
        }

        // 🔹 Simple SHA256 password hashing
        private string HashPassword(string password)
        {
            using var sha = SHA256.Create();
            var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(bytes);
        }

        public async Task<DataAccessLayer.DBContext.User?> UpdateUserAsync(UserCreateDto updatedUser)
        {
            var existingUser = await _context.Users.FindAsync(updatedUser.userId);
            if (existingUser == null) return null;

            existingUser.FullName = updatedUser.FullName;
            existingUser.Email = updatedUser.Email;
            existingUser.RoleId = updatedUser.RoleId;
            existingUser.ReportingTo = updatedUser.reportingTo;
            existingUser.DepartmentId = updatedUser.departmentId;
            existingUser.PasswordHash = updatedUser.Password;
            //existingUser.IsActive = updatedUser.IsActive;

            await _context.SaveChangesAsync();
            return existingUser;
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return false;

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task SendWelcomeEmailAsync(DataAccessLayer.DBContext.User user,string password)
        {
            try
            {
                using (var smtpClient = new SmtpClient())
                {
                    smtpClient.Host = _configuration["Smtp:Host"];
                    smtpClient.Port = int.Parse(_configuration["Smtp:Port"]);
                    smtpClient.EnableSsl = true;
                    smtpClient.UseDefaultCredentials = false;
                    smtpClient.Credentials = new NetworkCredential(
                        _configuration["Smtp:User"],
                        _configuration["Smtp:Password"]
                    );

                    string logoUrl = "http://qa-hr.cortracker360.com/assets/images/cor-logo.png"; // Replace with your actual logo

                    string subject = "Welcome to HRMS – Your Login Details";

                    string body = $@"
<!DOCTYPE html>
<html>
<head>
    <meta charset='UTF-8'>
    <title>Welcome to HRMS</title>
</head>
<body style='margin:0;padding:0;background-color:#f4f6f8;font-family:Segoe UI,Roboto,Helvetica,Arial,sans-serif;color:#333;'>
    <table role='presentation' cellpadding='0' cellspacing='0' width='100%' style='background-color:#f4f6f8;padding:40px 0;'>
        <tr>
            <td align='center'>
                <table role='presentation' cellpadding='0' cellspacing='0' width='600' style='background-color:#ffffff;border-radius:10px;overflow:hidden;box-shadow:0 2px 10px rgba(0,0,0,0.08);'>
                    <tr>
                        <td style='background-color:#004aad;padding:20px;text-align:center;'>
                            <img src='{logoUrl}' alt='Cortracker HRMS' style='height:60px;'/>
                        </td>
                    </tr>
                    <tr>
                        <td style='padding:40px 30px;'>
                            <h2 style='color:#004aad;margin-bottom:10px;'>Welcome to Cortracker HRMS, {user.FullName}!</h2>
                            <p style='font-size:16px;line-height:1.6;margin:20px 0;'>
                                We’re excited to have you onboard! Your HRMS account has been successfully created. Please find your login details below.
                            </p>

                            <table cellpadding='6' cellspacing='0' style='width:100%;margin:20px 0;border-collapse:collapse;'>
                                <tr>
                                    <td style='font-weight:bold;color:#004aad;width:120px;'>Login URL:</td>
                                    <td><a href='http://qa-hr.cortracker360.com' style='color:#004aad;text-decoration:none;'>https://qa-hr.cortracker360.com</a></td>
                                </tr>
                                <tr>
                                    <td style='font-weight:bold;color:#004aad;'>Username:</td>
                                    <td>{user.Email}</td>
                                </tr>
                                <tr>
                                    <td style='font-weight:bold;color:#004aad;'>Password:</td>
                                    <td>{password}</td>
                                </tr>
                            </table>

                            <p style='font-size:15px;line-height:1.6;'>
                                For your security, please update your password after your first login.
                            </p>

                            <div style='margin-top:30px;text-align:center;'>
                                <a href='http://qa-hr.cortracker360.com' 
                                   style='background-color:#004aad;color:#fff;padding:12px 24px;border-radius:6px;text-decoration:none;font-weight:600;'>
                                   Go to HRMS Portal
                                </a>
                            </div>

                            <p style='font-size:14px;color:#888;margin-top:30px;'>
                                Regards,<br/>
                                <strong>HR Team</strong><br/>
                                Cortracker HRMS
                            </p>
                        </td>
                    </tr>
                    <tr>
                        <td style='background-color:#f0f2f5;text-align:center;padding:15px;font-size:12px;color:#888;'>
                            © {DateTime.UtcNow.Year} Cortracker HRMS. All rights reserved.
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</body>
</html>";


                    var mailMessage = new MailMessage
                    {
                        From = new MailAddress(_configuration["Smtp:FromEmail"], "Cortracker HRMS"),
                        Subject = subject,
                        Body = body,
                        IsBodyHtml = true
                    };
                    mailMessage.To.Add(user.Email);

                    await smtpClient.SendMailAsync(mailMessage);
                }
            }
            catch (SmtpException smtpEx)
            {
                Console.WriteLine($"SMTP Error: {smtpEx.StatusCode} - {smtpEx.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Email sending failed: {ex.Message}");
            }
        }
        public async Task SendPasswordChangedEmailAsync(
    DataAccessLayer.DBContext.User user,
    string newPassword)
        {
            try
            {
                using (var smtpClient = new SmtpClient())
                {
                    smtpClient.Host = _configuration["Smtp:Host"];
                    smtpClient.Port = int.Parse(_configuration["Smtp:Port"]);
                    smtpClient.EnableSsl = true;
                    smtpClient.UseDefaultCredentials = false;
                    smtpClient.Credentials = new NetworkCredential(
                        _configuration["Smtp:User"],
                        _configuration["Smtp:Password"]
                    );

                    string logoUrl = "http://mock-hr.cortracker360.com/assets/images/cor-logo.png";

                    string subject = "Your HRMS Password Has Been Updated";

                    string body = $@"
<!DOCTYPE html>
<html>
<head>
    <meta charset='UTF-8'>
    <title>Password Changed</title>
</head>
<body style='margin:0;padding:0;background-color:#f4f6f8;font-family:Segoe UI,Roboto,Helvetica,Arial,sans-serif;color:#333;'>
    <table role='presentation' width='100%' cellpadding='0' cellspacing='0' style='padding:40px 0;'>
        <tr>
            <td align='center'>
                <table role='presentation' width='600' cellpadding='0' cellspacing='0'
                       style='background:#fff;border-radius:10px;box-shadow:0 2px 10px rgba(0,0,0,0.08);overflow:hidden;'>

                    <!-- Header -->
                    <tr>
                        <td style='background:#004aad;padding:20px;text-align:center;'>
                            <img src='{logoUrl}' alt='Cortracker HRMS' style='height:60px;' />
                        </td>
                    </tr>

                    <!-- Content -->
                    <tr>
                        <td style='padding:40px 30px;'>
                            <h2 style='color:#004aad;margin-bottom:10px;'>
                                Password Changed Successfully
                            </h2>

                            <p style='font-size:16px;line-height:1.6;margin:20px 0;'>
                                Hi <strong>{user.FullName}</strong>,<br/><br/>
                                Your HRMS account password has been updated successfully.
                                Below are your updated login credentials.
                            </p>

                            <table cellpadding='6' cellspacing='0'
                                   style='width:100%;margin:20px 0;border-collapse:collapse;'>
                                <tr>
                                    <td style='font-weight:bold;color:#004aad;width:130px;'>
                                        Login URL:
                                    </td>
                                    <td>
                                        <a href='https://qa-hr.cortracker360.com'
                                           style='color:#004aad;text-decoration:none;'>
                                           https://qa-hr.cortracker360.com
                                        </a>
                                    </td>
                                </tr>
                                <tr>
                                    <td style='font-weight:bold;color:#004aad;'>Username:</td>
                                    <td>{user.Email}</td>
                                </tr>
                                <tr>
                                    <td style='font-weight:bold;color:#004aad;'>Password:</td>
                                    <td>{newPassword}</td>
                                </tr>
                            </table>

                            <p style='font-size:15px;line-height:1.6;'>
                                If you did not initiate this change, please contact the HR
                                team immediately.
                            </p>

                            <div style='margin-top:30px;text-align:center;'>
                                <a href='https://qa-hr.cortracker360.com'
                                   style='background:#004aad;color:#fff;padding:12px 26px;
                                          border-radius:6px;text-decoration:none;font-weight:600;'>
                                   Login to HRMS
                                </a>
                            </div>

                            <p style='font-size:14px;color:#888;margin-top:30px;'>
                                Regards,<br/>
                                <strong>HR Team</strong><br/>
                                Cortracker HRMS
                            </p>
                        </td>
                    </tr>

                    <!-- Footer -->
                    <tr>
                        <td style='background:#f0f2f5;text-align:center;padding:15px;
                                   font-size:12px;color:#888;'>
                            © {DateTime.UtcNow.Year} Cortracker HRMS. All rights reserved.
                        </td>
                    </tr>

                </table>
            </td>
        </tr>
    </table>
</body>
</html>";

                    var mailMessage = new MailMessage
                    {
                        From = new MailAddress(_configuration["Smtp:FromEmail"], "Cortracker HRMS"),
                        Subject = subject,
                        Body = body,
                        IsBodyHtml = true
                    };

                    mailMessage.To.Add(user.Email);

                    await smtpClient.SendMailAsync(mailMessage);
                }
            }
            catch (SmtpException smtpEx)
            {
                Console.WriteLine($"SMTP Error: {smtpEx.StatusCode} - {smtpEx.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Email sending failed: {ex.Message}");
            }
        }

        public async Task<ApiResponse<bool>> SendOtpAsync(string email)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == email);

            if (user == null)
                return new ApiResponse<bool>(false, "Invalid Email", false);

            var otp = new Random().Next(100000, 999999).ToString();

            user.RefreshToken = otp;
            user.RefreshTokenExpiry = DateTime.UtcNow.AddMinutes(5);

            await _context.SaveChangesAsync();

            await SendOtpEmailAsync(user, otp);

            return new ApiResponse<bool>(true, "OTP sent successfully", true);
        }

        public async Task<ApiResponse<bool>> VerifyOtpAsync(string email, string otp)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(x => x.Email == email);

            if (user == null)
                return new ApiResponse<bool>(false, "Invalid Email", false);

            if (user.RefreshToken != otp ||
                user.RefreshTokenExpiry < DateTime.UtcNow)
            {
                return new ApiResponse<bool>(false, "Invalid or Expired OTP", false);
            }

            return new ApiResponse<bool>(true, "OTP Verified", true);
        }
        public async Task<ApiResponse<bool>> ResetPasswordAsync(string email, string newPassword)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(x => x.Email == email);

            if (user == null)
                return new ApiResponse<bool>(false, "Invalid Email", false);

            user.PasswordHash = newPassword;
            user.Passwordchanged = true;

            user.RefreshToken = null;
            user.RefreshTokenExpiry = null;

            await _context.SaveChangesAsync();

            await SendPasswordChangedEmailAsync(user, newPassword);

            return new ApiResponse<bool>(true, "Password reset successful", true);
        }

        private async Task SendOtpEmailAsync(DataAccessLayer.DBContext.User user, string otp)
        {
            string subject = "Your OTP for Password Reset";

            string body = $@"
        <h3>Hello {user.FullName},</h3>
        <p>Your OTP for password reset is:</p>
        <h2>{otp}</h2>
        <p>This OTP is valid for 5 minutes.</p>
        <br/>
        <p>Regards,<br/>HR Team</p>
    ";

            using var smtpClient = new SmtpClient
            {
                Host = _configuration["Smtp:Host"],
                Port = int.Parse(_configuration["Smtp:Port"]),
                EnableSsl = true,
                Credentials = new NetworkCredential(
                    _configuration["Smtp:User"],
                    _configuration["Smtp:Password"]
                )
            };

            var mail = new MailMessage(
                _configuration["Smtp:FromEmail"],
                user.Email,
                subject,
                body
            )
            { IsBodyHtml = true };

            await smtpClient.SendMailAsync(mail);
        }

    }
}

