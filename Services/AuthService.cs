using API_Project.Data;
using API_Project.Enums;
using API_Project.Helpers;
using API_Project.Models.DTOs;

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

        public string Login(LoginModel model)
        {
            var user = _db.Users.FirstOrDefault(u => u.Phone == model.Phone);
            if (user == null || user.Password != model.Password)
                return null;
            string roleName = ((UserRole)user.Role).ToString();
            return _tokenGenerator.GenerateToken(user.Phone, roleName);
        }
    }
}
