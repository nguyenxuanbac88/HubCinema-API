using API_Project.Enums;
using API_Project.Models.DTOs;
using API_Project.Services;
using Microsoft.AspNetCore.Mvc;

namespace API_Project.Controllers
{
    [ApiController]
    [Route("api/user")]
    public class UserController : ControllerBase
    {
        private readonly Profile _profileService;

        public UserController(Profile profileService)
        {
            _profileService = profileService;
        }

        [HttpGet("get-info/{token}")]
        public async Task<IActionResult> GetInfo(string token)
        {
            return await _profileService.HandleGetUserInfoAsync(token);
        }

        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePw([FromBody] ChangePwDTO model)
        {
            var (result, message) = await _profileService.ChangePasswordAsync(model);

            return result switch
            {
                ChangePasswordResult.Success => Ok(new { message }),
                ChangePasswordResult.MissingInput => BadRequest(message),
                ChangePasswordResult.InvalidToken => Unauthorized(message),
                ChangePasswordResult.WrongOldPassword => BadRequest(message),
                ChangePasswordResult.InvalidNewPassword => BadRequest(message),
                _ => StatusCode(500, "Có lỗi xảy ra khi đổi mật khẩu.")
            };
        }
    }
}
