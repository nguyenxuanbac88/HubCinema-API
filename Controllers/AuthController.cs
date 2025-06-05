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
        public IActionResult Login([FromBody] LoginModel model)
        {
            var token = _authService.Login(model);

            if (token == null)
                return Unauthorized("Sai tên đăng nhập hoặc mật khẩu");

            return Ok(new { token });
        }
    }
}
