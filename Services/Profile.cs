using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using API_Project.Data;
using API_Project.Models;
using API_Project.Models.Entities;
using System;

namespace API_Project.Services
{
    public class Profile
    {
        private readonly ApplicationDbContext _db;

        public Profile(ApplicationDbContext db)
        {
            _db = db;
        }

        // Lấy username (phone/email) từ JWT token
        public string GetUsernameFromToken(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            var username = jwtToken.Claims.FirstOrDefault(c =>
                c.Type == System.Security.Claims.ClaimTypes.Name || c.Type == "unique_name")?.Value;

            return username;
        }

        // Lấy thông tin người dùng từ token và kiểm tra Token có khớp với DB không
        public async Task<User> GetUserFromTokenAsync(string token)
        {
            var username = GetUsernameFromToken(token);
            if (string.IsNullOrEmpty(username))
                return null;

            var user = await _db.Users.FirstOrDefaultAsync(u =>
                (u.Phone == username || u.Email == username) &&
                u.TokenLogin == token
            );

            return user;
        }
    }
}
