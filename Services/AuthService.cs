using API_Project.Data;
using API_Project.Enums;
using API_Project.Helpers;
using API_Project.Models.DTOs;
using API_Project.Models.Entities;

namespace API_Project.Services
{
    public class AuthService
    {
        private readonly ApplicationDbContext _db;
        private readonly JwtTokenGenerator _tokenGenerator;

        public AuthService(ApplicationDbContext db, JwtTokenGenerator tokenGenerator)
        {
            _db = db;
            _tokenGenerator = tokenGenerator;
        }

        public string Login(LoginDTO model)
        {
            var user = _db.Users
                .FirstOrDefault(u => u.Phone == model.Username || u.Email == model.Username);

            if (user == null)
                return null;

            if (!PasswordHasher.VerifyPassword(model.Password, user.Password))
                return null;

            string roleName = ((UserRole)user.Role).ToString();
            return _tokenGenerator.GenerateToken(user.Phone, roleName);
        }
        public RegisterResult Register(RegisterDTO model)
        {
            //Check độ tuổi đăng ký, phải đủ 12 tuổi bao gồm cả ngày
            var age = DateTime.Now.Year - model.dob.Year;
            if (DateTime.Now.Date < model.dob.Date.AddYears(age)) age--;
            if(age<12)
                return RegisterResult.Underage;
            //Kiểm tra SDT và Email đã tồn tại trong CSDL chưa ?
            if (_db.Users.Any(u=> u.Phone == model.phone || u.Email == model.email))
                return RegisterResult.PhoneOrEmailExists;

            //Dùng hàm mã hoá để mã hoá mật khẩu
            string hashedPassword = PasswordHasher.HashPassword(model.password);
            var user = new User
            {
                Phone = model.phone,
                Email = model.email,
                Password = hashedPassword,
                FullName = model.name,
                Dob = model.dob,
                Gender = model.gender,
                ZoneAddress = model.zoneAddress,
                Role = 0,
                Points = 0,
                TotalSpending = 0
            };
            _db.Users.Add(user);
            _db.SaveChanges();

            //Thêm lớp ảo ID cho User
            user.UserCode = GenerateUserCode(user.IDUser);
            return RegisterResult.Success;
        }
        private string GenerateUserCode(int idUser)
        {
            var ms = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            var msStr = ms.ToString();
            var msCut = msStr.Length > 2 ? msStr.Substring(2) : msStr;
            return msCut + idUser.ToString();
        }


    }
}
