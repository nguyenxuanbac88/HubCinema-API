using API_Project.Models.DTOs;
using API_Project.Models.Entities;
using API_Project.Services;
using Microsoft.AspNetCore.Mvc;

namespace API_Project.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginDTO model)
        {
            var token = _authService.Login(model);

            if (token == null)
                return Unauthorized("Sai tên đăng nhập hoặc mật khẩu");

            return Ok(new { token });
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] RegisterDTO model)
        {
            var result = _authService.Register(model);
            switch (result)
            {
                case Enums.RegisterResult.Underage:
                    return BadRequest("Bạn chưa đủ 12 tuổi để đăng ký.");
                case Enums.RegisterResult.PhoneOrEmailExists:
                    return BadRequest("Số điện thoại hoặc email đã tồn tại.");
                case Enums.RegisterResult.Success:
                    return Ok(new { message = "Đăng ký thành công!" });
                default:
                    return StatusCode(500, "Có lỗi xảy ra.");
            }
        }

        [HttpPost("forgot-password")]
        public IActionResult ForgotPassword([FromBody] FogotPasswordDTO model)
        {
            bool result = _authService.FogotPassword(model);

            if (!result)
                return BadRequest("Tài khoản không hợp lệ hoặc email không đúng định dạng.");

            return Ok(new { message = "Mã OTP đã được gửi đến email của bạn." });
        }
        [HttpPost("confirm-password")]
        public IActionResult ConfirmPassword([FromBody] ConfirmPwDTO model)
        {
            bool result = _authService.ConfirmPW(model);

            if (!result)
                return BadRequest("Sai mã OTP hoặc mã OTP đã hết hạn");

            return Ok(new { message = "Đổi mật khẩu thành công" });
        }
    }
}
