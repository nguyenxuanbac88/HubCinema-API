using API_Project.Data;
using API_Project.Enums;
using API_Project.Helpers;
using API_Project.Models.DTOs;
using API_Project.Models.Entities;
using System;
using System.Linq;

namespace API_Project.Services
{
    public class AuthService
    {
        private readonly ApplicationDbContext _db;
        private readonly JwtTokenGenerator _tokenGenerator;
        private readonly EmailService _emailService;

        public AuthService(ApplicationDbContext db, JwtTokenGenerator tokenGenerator, EmailService emailService)
        {
            _db = db;
            _tokenGenerator = tokenGenerator;
            _emailService = emailService;
        }

        public (AuthResult result, string token) Login(LoginDTO model)
        {
            var user = _db.Users
                .FirstOrDefault(u => u.Phone == model.Username || u.Email == model.Username);

            if (user == null || !PasswordHasher.VerifyPassword(model.Password, user.Password))
                return (AuthResult.InvalidCredentials, null);

            string roleName = ((UserRole)user.Role).ToString();
            string token = _tokenGenerator.GenerateToken(user.Phone, roleName);

            user.TokenLogin = token;
            _db.SaveChanges();

            return (AuthResult.Success, token);
        }

        public RegisterResult Register(RegisterDTO model)
        {
            // Kiểm tra tuổi
            int age = DateTime.Today.Year - model.dob.Year;
            if (model.dob.Date > DateTime.Today.AddYears(-age)) age--;
            if (age < 12)
                return RegisterResult.Underage;

            // Kiểm tra email
            var emailStatus = CheckAuth.CheckEmail(model.email);
            if (emailStatus != EmailCheckResult.Valid)
                return RegisterResult.InvalidEmail;

            // Kiểm tra số điện thoại
            var phoneStatus = CheckAuth.CheckPhoneNumber(model.phone);
            if (phoneStatus != PhoneCheckResult.Valid)
                return RegisterResult.InvalidPhone;

            // Kiểm tra mật khẩu
            var pwStatus = CheckAuth.CheckPassword(model.password);
            if (pwStatus != PasswordCheckResult.Valid)
                return RegisterResult.InvalidPassword;

            // Kiểm tra tài khoản tồn tại
            var exists = _db.Users.Any(u => u.Email == model.email || u.Phone == model.phone);
            if (exists)
                return RegisterResult.AccountExists;

            // Mã hoá mật khẩu
            string hashedPassword = PasswordHasher.HashPassword(model.password);

            var user = new User
            {
                Phone = model.phone,
                Email = model.email,
                Password = hashedPassword,
                FullName = model.name,
                Dob = model.dob,
                Gender = Convert.ToBoolean(model.gender),
                ZoneAddress = model.zoneAddress,
                Role = 0,
                Points = 0,
                TotalSpending = 0
            };

            _db.Users.Add(user);
            _db.SaveChanges();

            // Sinh mã barcode từ IDUser sau khi lưu
            user.MaBarcode = GenerateUserCode(user.IDUser);
            _db.SaveChanges();

            return RegisterResult.Success;
        }

        private string GenerateUserCode(int idUser)
        {
            var ms = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            var msStr = ms.ToString();
            var msCut = msStr.Length > 2 ? msStr.Substring(2) : msStr;
            return msCut + idUser.ToString();
        }

        public AuthResult FogotPassword(FogotPasswordDTO model)
        {
            var user = _db.Users
                .FirstOrDefault(u => u.Phone == model.Username || u.Email == model.Username);

            if (user == null)
                return AuthResult.UserNotFound;

            var ResultsEmail = CheckAuth.CheckEmail(model.Username);
            if ((int)ResultsEmail!=0)
                return AuthResult.EmailInvalid;

            var otp = GenerateOTP.GenerateUserOTP();

            string subject = "Mã OTP HubCinema";
            string body = $@"
                <div style='font-family: Arial, sans-serif; line-height: 1.6;'>
                    <h2>HubCinema - Xác nhận tài khoản</h2>
                    <p>Xin chào <b>{user.FullName}</b>,</p>
                    <p>Bạn (hoặc ai đó) đã yêu cầu đặt lại mật khẩu cho tài khoản HubCinema của bạn.</p>
                    <p>Mã OTP của bạn là:</p>
                    <h3 style='color: #e91e63;'>{otp}</h3>
                    <p>Mã này có hiệu lực trong vòng <b>15 phút</b>.</p>
                    <p>Nếu bạn không yêu cầu điều này, hãy bỏ qua email này.</p>
                    <br/>
                    <p>Trân trọng,<br/>HubCinema</p>
                </div>";

            _emailService.SendEmail(model.Username, subject, body);

            user.OTP = PasswordHasher.HashPassword(otp);
            user.TimeOtp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            _db.SaveChanges();

            return AuthResult.Success;
        }

        public AuthResult ConfirmPW(ConfirmPwDTO model)
        {
            var user = _db.Users
                .FirstOrDefault(u => u.Phone == model.Username || u.Email == model.Username);

            if (user == null)
                return AuthResult.UserNotFound;

            if (user.OTP != PasswordHasher.HashPassword(model.OTP))
                return AuthResult.OtpInvalid;

            bool isExpired = (DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() - user.TimeOtp) > (15 * 60 * 1000);
            if (isExpired)
                return AuthResult.OtpExpired;

            user.Password = PasswordHasher.HashPassword(model.NewPW);
            _db.SaveChanges();

            return AuthResult.Success;
        }
    }
}
