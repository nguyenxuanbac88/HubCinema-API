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

namespace API_Project.Services
{
    public class Profile
    {
        private readonly ApplicationDbContext _db;

        public Profile(ApplicationDbContext db)
        {
            _db = db;
        }

        public string GetUsernameFromToken(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            var username = jwtToken.Claims.FirstOrDefault(c =>
                c.Type == System.Security.Claims.ClaimTypes.Name || c.Type == "unique_name")?.Value;

            return username;
        }

        public async Task<User> GetUserFromTokenAsync(string token)
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

            if (user.Password != model.OldPassword)
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

            user.Password = model.NewPassword;
            await _db.SaveChangesAsync();

            return (ChangePasswordResult.Success, "Đổi mật khẩu thành công!");
        }

    }
}
