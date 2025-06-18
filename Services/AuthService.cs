using API_Project.Data;
using API_Project.Enums;
using API_Project.Helpers;
using API_Project.Services;
using API_Project.Models.DTOs;
using API_Project.Models.Entities;
using API_Project.Models;

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

        public string Login(LoginDTO model)
        {
            var user = _db.Users
                .FirstOrDefault(u => u.Phone.Trim() == model.Username.Trim() || u.Email.Trim() == model.Username.Trim());

            if (user == null)
                return null;

            if (!PasswordHasher.VerifyPassword(model.Password.Trim(), user.Password.Trim()))
                return null;

            string roleName = ((UserRole)user.Role).ToString();
            string toKen = _tokenGenerator.GenerateToken(user.Phone, roleName);
            user.TokenLogin = toKen;
            _db.SaveChanges();
            return toKen;
        }

        public RegisterResult Register(RegisterDTO model)
        {
            var age = DateTime.Now.Year - model.dob.Year;
            if (DateTime.Now.Date < model.dob.Date.AddYears(age)) age--;
            if (age < 12)
                return RegisterResult.Underage;

            if (_db.Users.Any(u => u.Phone.Trim() == model.phone.Trim() || u.Email.Trim() == model.email.Trim()))
                return RegisterResult.PhoneOrEmailExists;

            string hashedPassword = PasswordHasher.HashPassword(model.password).Trim();

            var user = new User
            {
                Phone = model.phone?.Trim(),
                Email = model.email?.Trim(),
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

            user.MaBarcode = GenerateUserCode(user.IDUser).Trim();
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

        public bool FogotPassword(FogotPasswordDTO model)
        {
            string subject = "Mã OTP HubCinema";

            var user = _db.Users
                  .FirstOrDefault(u => u.Phone.Trim() == model.Username.Trim() || u.Email.Trim() == model.Username.Trim());
            if (user == null || !EmailValidator.IsValidEmail(model.Username))
                return false;

            var otp = GenerateOTP.GenerateUserOTP().Trim();

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

            user.OTP = PasswordHasher.HashPassword(otp).Trim();
            user.TimeOtp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            _db.SaveChanges();
            return true;
        }
        public bool ConfirmPW(ConfirmPwDTO model)
        {
            var user = _db.Users
                  .FirstOrDefault(u => u.Phone.Trim() == model.Username.Trim() || u.Email.Trim() == model.Username.Trim());
            long now = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            if (user == null)
                return false;

            if (user.OTP != PasswordHasher.HashPassword(model.OTP))
                return false;

            bool isExpired = (now - user.TimeOtp) > (15 * 60 * 1000);
            if (isExpired == true)
                return false;
            user.Password = PasswordHasher.HashPassword(model.NewPW);
            _db.SaveChanges();
            return true;
        }
    }
}
