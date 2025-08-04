using API_Project.Models.DTOs;

namespace API_Project.AdminServices
{
    public interface IUserAdminService
    {
        Task<List<UserDto>> GetAllUsersAsync(string? keyword);
    }
}
