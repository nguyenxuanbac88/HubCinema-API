using API_Project.Enums;
using API_Project.Models.DTOs;
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
            var (result, token) = _authService.Login(model);

            return result switch
            {
                AuthResult.Success => Ok(new { token }),
                AuthResult.InvalidCredentials => Unauthorized("Sai tên đăng nhập hoặc mật khẩu."),
                _ => StatusCode(500, "Có lỗi xảy ra trong quá trình đăng nhập.")
            };
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] RegisterDTO model)
        {
            var result = _authService.Register(model);

            return result switch
            {
                RegisterResult.Success => Ok(new { message = "Đăng ký thành công!" }),
                RegisterResult.Underage => BadRequest("Bạn chưa đủ 12 tuổi để đăng ký."),
                RegisterResult.InvalidEmail => BadRequest("Email không đúng định dạng."),
                RegisterResult.InvalidPhone => BadRequest("Số điện thoại không hợp lệ."),
                RegisterResult.InvalidPassword => BadRequest("Mật khẩu không đúng định dạng."),
                RegisterResult.AccountExists => BadRequest("Số điện thoại hoặc email đã tồn tại."),
                _ => StatusCode(500, "Có lỗi xảy ra trong quá trình đăng ký.")
            };
        }

        [HttpPost("forgot-password")]
        public IActionResult ForgotPassword([FromBody] FogotPasswordDTO model)
        {
            var result = _authService.FogotPassword(model);

            return result switch
            {
                AuthResult.Success => Ok(new { message = "Mã OTP đã được gửi đến email của bạn." }),
                AuthResult.UserNotFound => NotFound("Không tìm thấy người dùng."),
                AuthResult.EmailInvalid => BadRequest("Email không đúng định dạng."),
                _ => StatusCode(500, "Gửi mã OTP thất bại.")
            };
        }

        [HttpPost("confirm-password")]
        public IActionResult ConfirmPassword([FromBody] ConfirmPwDTO model)
        {
            var result = _authService.ConfirmPW(model);

            return result switch
            {
                AuthResult.Success => Ok(new { message = "Đổi mật khẩu thành công." }),
                AuthResult.UserNotFound => NotFound("Tài khoản không tồn tại."),
                AuthResult.OtpInvalid => BadRequest("Mã OTP không đúng."),
                AuthResult.OtpExpired => BadRequest("Mã OTP đã hết hạn."),
                _ => StatusCode(500, "Xác nhận đổi mật khẩu thất bại.")
            };
        }
    }
}
