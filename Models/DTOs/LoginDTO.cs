using System.ComponentModel.DataAnnotations;

namespace API_Project.Models.DTOs
{
    public class LoginDTO
    {
        [Required(ErrorMessage = "Vui lòng nhập tài khoản.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập mật khẩu.")]
        [MinLength(6, ErrorMessage = "Mật khẩu phải từ 6 kí tự trở lên")]
        [MaxLength(50, ErrorMessage = "Vui lòng nhập mật khẩu hợp lệ")]
        public string Password { get; set; }
    }
}
