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

        public UserController(UserService profileService)
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

        [HttpPost("ChangeEmailRequest")]
        public async Task<IActionResult> ChangeEmailRequest([FromBody] ChangeEmailRequestDTO model)
        {
            var (result, message) = await _profileService.ChangeEmail(model, HttpContext);

            return result switch
            {
                ChangeEmailResult.SuccessSendEmail => Ok(new { result, message }),
                ChangeEmailResult.InvalidToken => Unauthorized(new { result, message }),
                ChangeEmailResult.InvalidFormatEmail => BadRequest(new { result, message }),
                ChangeEmailResult.SameEmail => BadRequest(new { result, message }),
                ChangeEmailResult.EmailAlreadyUsed => Conflict(new { result, message }),
                _ => StatusCode(500, new { result, message = "Lỗi không xác định." })
            };
        }

        [HttpPost("ChangeEmailConfirm")]
        public async Task<IActionResult> ChangeEmailConfirm([FromBody] ChangeEmailConfirmDTO model)
        {
            var (result, message) = await _profileService.ChangeEmailConfirm(model, HttpContext);

            return result switch
            {
                ChangeEmailResult.SuccessConfirm => Ok(new { result, message }),
                ChangeEmailResult.InvalidToken => Unauthorized(new { result, message }),
                ChangeEmailResult.OtpInvalid => BadRequest(new { result, message }),
                ChangeEmailResult.OtpExpired => BadRequest(new { result, message }),
                _ => StatusCode(500, new { result, message = "Lỗi không xác định." })
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
