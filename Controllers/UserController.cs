using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using API_Project.Services;
using API_Project.Enums;

namespace API_Project.Controllers
{
    [ApiController]
    [Route("Api/User")]
    public class UserController : Controller
    {
        private readonly Profile _profileService;

        public UserController(Profile profileService)
        {
            _profileService = profileService;
        }

        [HttpGet("GetInfo/{token}")]
        public async Task<IActionResult> GetInfo(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
                return BadRequest("Thiếu token");

            var user = await _profileService.GetUserFromTokenAsync(token);

            if (user == null)
                return Unauthorized("Token không hợp lệ hoặc người dùng không tồn tại");

            return Ok(new
            {
                user.IDUser,
                user.MaBarcode,
                user.FullName,
                user.Phone,
                user.Email,
                Dob = user.Dob.ToString("yyyy-MM-dd"),
                Gender = user.Gender ? "Nam" : "Nữ",
                Role = user.Role,
                Diem = user.Points,
                TongChiTieu = user.TotalSpending,
                ThuHang = user.Rank,
                KhuVuc = user.ZoneAddress
            });
        }

    }
}
