using API_Project.Models.DTOs;
using API_Project.Services;
using Microsoft.AspNetCore.Mvc;

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

        [HttpGet("GetInfo")]
        public async Task<IActionResult> GetInfo()
        {
            var result = await _profileService.HandleGetUserInfoAsync(HttpContext);
            return result;
        }

        [HttpPost("ChangePw")]
        public async Task<IActionResult> ChangePw([FromBody] ChangePwDTO model)
        {
            var (status, message) = await _profileService.ChangePasswordAsync(model, HttpContext);
            return Ok(new { status, message });
        }
        [HttpGet("Logout")]
        public async Task<IActionResult> Logout()
        {
            var result = await _profileService.HandleLogoutAsync(HttpContext);
            return result;
        }
    }
}