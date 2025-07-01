using API_Project.Enums;
using API_Project.Models.DTOs;
using API_Project.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace API_Project.Controllers
{
    [ApiController]
    [Route("Api/User")]
    public class UserController : Controller
    {
        private readonly UserService _profileService;
        private readonly AuthService _authService;

        public UserController(UserService profileService, AuthService authService)
        {
            _profileService = profileService;
            _authService = authService;
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

        [HttpPost("ChangeEmail")]
        public async Task<IActionResult> ChangeEmail([FromBody] ChangeEmailRequestDTO model)
        {
            var (result, message, otpToken) = await _profileService.ChangeEmail(model, HttpContext);

            return result switch
            {
                ChangeEmailResult.SuccessSendEmail => Ok(new { message, otpToken }),
                ChangeEmailResult.InvalidToken => Unauthorized(new { message }),
                ChangeEmailResult.InvalidFormatEmail => BadRequest(new { message }),
                ChangeEmailResult.SameEmail => BadRequest(new { message }),
                ChangeEmailResult.EmailAlreadyUsed => Conflict(new { message }),
                _ => StatusCode(500, new { message = "Gửi OTP thất bại." })
            };
        }


        [HttpPost("ConfirmChangeEmail")]
        public async Task<IActionResult> ConfirmChangeEmail([FromBody] ChangeEmailConfirmDTO model)
        {
            var (result, message) = await _profileService.ChangeEmailConfirm(model, HttpContext);

            return result switch
            {
                ChangeEmailResult.SuccessConfirm => Ok(new { message }),
                ChangeEmailResult.InvalidToken => Unauthorized(new { message }),
                ChangeEmailResult.OtpInvalid => BadRequest(new { message }),
                ChangeEmailResult.OtpExpired => BadRequest(new { message }),
                _ => StatusCode(500, new { message = "Xác nhận OTP thất bại." })
            };
        }

        [HttpGet("Logout")]
        public async Task<IActionResult> Logout()
        {
            var result = await _profileService.HandleLogoutAsync(HttpContext);
            return result;
        }
    }
}
