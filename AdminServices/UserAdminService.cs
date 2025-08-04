using API_Project.Data;
using API_Project.Models.DTOs;
using Microsoft.EntityFrameworkCore;

namespace API_Project.AdminServices
{
    public class UserAdminService : IUserAdminService
    {
        private readonly ApplicationDbContext _context;

        public UserAdminService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<UserDto>> GetAllUsersAsync(string? keyword)
        {
            var query = _context.Users.AsQueryable();

            if (!string.IsNullOrEmpty(keyword))
            {
                keyword = keyword.ToLower();
                query = query.Where(u =>
                    u.Email.ToLower().Contains(keyword) ||
                    u.Phone.Contains(keyword));
            }

            var users = await query
                .Select(u => new UserDto
                {
                    Id = u.IDUser,
                    FullName = u.FullName,
                    Email = u.Email,
                    Phone = u.Phone,
                    Role = u.Role == 1 ? "Admin" : "User",
                    CreateAt = u.CreateAt
                })
                .ToListAsync();

            return users;
        }
    }

}
