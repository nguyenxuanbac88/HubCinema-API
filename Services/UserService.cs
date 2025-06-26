using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using API_Project.Data;
using API_Project.Models.Entities;
using API_Project.Models.DTOs;
using API_Project.Enums;
using API_Project.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Net.NetworkInformation;

namespace API_Project.Services
{
    public class UserService
    {
        private readonly ApplicationDbContext _db;
        private readonly EmailService _emailService;
        public UserService(ApplicationDbContext db, EmailService emailService)
        {
            _db = db;
            _emailService = emailService;
        }

        public string GetUsernameFromToken(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            var username = jwtToken.Claims.FirstOrDefault(c =>
                c.Type == System.Security.Claims.ClaimTypes.Name || c.Type == "unique_name")?.Value;

            return username;
        }

        public async Task<Models.Entities.User> GetUserFromTokenAsync(string token)
        {
            var username = GetUsernameFromToken(token);
            if (string.IsNullOrEmpty(username))
                return null;

            return await _db.Users.FirstOrDefaultAsync(u =>
                (u.Phone == username || u.Email == username) &&
                u.TokenLogin == token
            );
        }

        // ✅ Sửa lại: Lấy token từ Header "Authorization: Bearer ..."
        public async Task<IActionResult> HandleGetUserInfoAsync(HttpContext httpContext)
        {
            var authHeader = httpContext.Request.Headers["Authorization"].FirstOrDefault();
            if (string.IsNullOrWhiteSpace(authHeader) || !authHeader.StartsWith("Bearer "))
                return new BadRequestObjectResult("Thiếu token hoặc định dạng không đúng");

            var token = authHeader.Substring("Bearer ".Length).Trim();

            var user = await GetUserFromTokenAsync(token);

            if (user == null)
                return new UnauthorizedObjectResult("Token không hợp lệ hoặc người dùng không tồn tại");

            return new OkObjectResult(new
            {
                user.IDUser,
                user.MaBarcode,
                user.FullName,
                user.Phone,
                user.Email,
                UserRole = user.Role,
                Dob = user.Dob.ToString("yyyy-MM-dd"),
                Gender = user.Gender ? "Nam" : "Nữ",
                Diem = user.Points,
                TongChiTieu = user.TotalSpending,
                ThuHang = user.Rank,
                KhuVuc = user.ZoneAddress
            });
        }

        // Đổi mật khẩu giữ nguyên
        public async Task<(ChangePasswordResult result, string message)> ChangePasswordAsync(ChangePwDTO model, HttpContext httpContext)
        {
            if (model == null ||
                string.IsNullOrWhiteSpace(model.OldPassword) ||
                string.IsNullOrWhiteSpace(model.NewPassword))
            {
                return (ChangePasswordResult.MissingInput, "Thiếu thông tin đầu vào.");
            }

            // ✅ Lấy token từ header
            var authHeader = httpContext.Request.Headers["Authorization"].FirstOrDefault();
            if (string.IsNullOrWhiteSpace(authHeader) || !authHeader.StartsWith("Bearer "))
                return (ChangePasswordResult.InvalidToken, "Thiếu token hoặc định dạng không hợp lệ.");

            var token = authHeader.Substring("Bearer ".Length).Trim();

            var user = await GetUserFromTokenAsync(token);
            if (user == null)
            {
                return (ChangePasswordResult.InvalidToken, "Token không hợp lệ hoặc người dùng không tồn tại.");
            }

            if (user.Password != PasswordHasher.HashPassword(model.OldPassword))
            {
                return (ChangePasswordResult.WrongOldPassword, "Mật khẩu cũ không chính xác.");
            }

            var pwStatus = CheckAuth.CheckPassword(model.NewPassword);
            if (pwStatus != PasswordCheckResult.Valid)
            {
                string msg = pwStatus switch
                {
                    PasswordCheckResult.TooShort => "Mật khẩu quá ngắn (tối thiểu 7 ký tự).",
                    PasswordCheckResult.TooLong => "Mật khẩu quá dài (tối đa 16 ký tự).",
                    PasswordCheckResult.MissingUppercase => "Phải có ít nhất một chữ in hoa.",
                    PasswordCheckResult.MissingLowercase => "Phải có ít nhất một chữ thường.",
                    PasswordCheckResult.MissingDigit => "Phải có ít nhất một số.",
                    PasswordCheckResult.MissingSpecialChar => "Phải có ít nhất một ký tự đặc biệt.",
                    PasswordCheckResult.ContainsInvalidChar => "Mật khẩu chứa ký tự không hợp lệ.",
                    _ => "Mật khẩu không hợp lệ."
                };

                return (ChangePasswordResult.InvalidNewPassword, msg);
            }

            user.Password = PasswordHasher.HashPassword(model.NewPassword);
            await _db.SaveChangesAsync();

            return (ChangePasswordResult.Success, "Đổi mật khẩu thành công!");
        }
        public async Task<(ChangeEmailResult result, string message)> ChangeEmail(ChangeEmailRequestDTO model, HttpContext httpContext)
        {
            var authHeader = httpContext.Request.Headers["Authorization"].FirstOrDefault();
            if (string.IsNullOrWhiteSpace(authHeader) || !authHeader.StartsWith("Bearer "))
                return (ChangeEmailResult.InvalidToken, "Thiếu token hoặc định dạng không hợp lệ.");

            var token = authHeader.Substring("Bearer ".Length).Trim();

            var user = await GetUserFromTokenAsync(token);
            if (user == null)
            {
                return (ChangeEmailResult.InvalidToken, "Token không hợp lệ hoặc người dùng không tồn tại.");
            }

            var ResultsEmail = CheckAuth.CheckEmail(model.EmailNew);
            if (ResultsEmail != EmailCheckResult.Valid)
            {
                string msg = ResultsEmail switch
                {
                    EmailCheckResult.Empty => "Email mới không được trống",
                    EmailCheckResult.InvalidFormat => "Email không đúng định dạng",
                    _ => "Email không hợp lệ."
                };

                return (ChangeEmailResult.InvalidFormatEmail, msg);
            }
            if (model.EmailNew == user.Email)
                return (ChangeEmailResult.SameEmail, "Email mới không được trùng với email hiện tại.");

            var emailExists = await _db.Users.AnyAsync(u => u.Email == model.EmailNew && u.IDUser != user.IDUser);
            if (emailExists)
                return (ChangeEmailResult.EmailAlreadyUsed, "Email này đã được sử dụng bởi tài khoản khác.");


            var otp = GenerateOTP.GenerateUserOTP();

            string subject = "Mã OTP HubCinema";
            string body = $@"
                <div style='font-family: Arial, sans-serif; line-height: 1.6;'>
                    <h2>HubCinema - Xác nhận tài khoản</h2>
                    <p>Xin chào <b>{user.FullName}</b>,</p>
                    <p>Bạn (hoặc ai đó) đã yêu cầu đổi Email cho tài khoản HubCinema của bạn.</p>
                    <p>Mã OTP của bạn là:</p>
                    <h3 style='color: #e91e63;'>{otp}</h3>
                    <p>Mã này có hiệu lực trong vòng <b>15 phút</b>.</p>
                    <p>Nếu bạn không yêu cầu điều này, hãy bỏ qua email này.</p>
                    <br/>
                    <p>Trân trọng,<br/>HubCinema</p>
                </div>";

            _emailService.SendEmail(model.EmailNew, subject, body);

            user.OTP = PasswordHasher.HashPassword(otp);
            user.TimeOtp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            user.EmailPending = model.EmailNew;
            await _db.SaveChangesAsync();
            return (ChangeEmailResult.SuccessSendEmail, "Đã gửi OTP về Email mới!");
        }
        public async Task<(ChangeEmailResult result, string message)> ChangeEmailConfirm(ChangeEmailConfirmDTO model, HttpContext httpContext)
        {
            var authHeader = httpContext.Request.Headers["Authorization"].FirstOrDefault();
            if (string.IsNullOrWhiteSpace(authHeader) || !authHeader.StartsWith("Bearer "))
                return (ChangeEmailResult.InvalidToken, "Thiếu token hoặc định dạng không hợp lệ.");

            var token = authHeader.Substring("Bearer ".Length).Trim();

            var user = await GetUserFromTokenAsync(token);
            if (user == null)
            {
                return (ChangeEmailResult.InvalidToken, "Token không hợp lệ hoặc người dùng không tồn tại.");
            }
            //Kiểm tra OTP có khớp không?
            if (!PasswordHasher.VerifyPassword(model.Otp, user.OTP))
            {
                return (ChangeEmailResult.OtpInvalid, "Mã OTP không hợp lệ");
            }

            //Kiểm tra token còn hạn không
            bool isExpired = (DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() - user.TimeOtp) > (15 * 60 * 1000);
            if (isExpired)
            {
                return (ChangeEmailResult.OtpExpired, "Mã OTP đã hết hạn");
            }
            user.Email = user.EmailPending;
            user.EmailPending = null;

            await _db.SaveChangesAsync();
            return (ChangeEmailResult.SuccessConfirm, "Đổi Email thành công!");
        }
        public async Task<IActionResult> HandleLogoutAsync(HttpContext httpContext)
        {
            var authHeader = httpContext.Request.Headers["Authorization"].FirstOrDefault();
            if (string.IsNullOrWhiteSpace(authHeader) || !authHeader.StartsWith("Bearer "))
                return new BadRequestObjectResult("Thiếu token hoặc định dạng không đúng");

            var token = authHeader.Substring("Bearer ".Length).Trim();

            var user = await _db.Users.FirstOrDefaultAsync(u => u.TokenLogin == token);
            if (user == null)
                return new UnauthorizedObjectResult("Token không hợp lệ");

            // Xoá hoặc reset token
            user.TokenLogin = null;
            await _db.SaveChangesAsync();

            return new OkObjectResult(new { message = "Đăng xuất thành công" });
        }

    }
}
