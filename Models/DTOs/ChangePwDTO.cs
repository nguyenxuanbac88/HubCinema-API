namespace API_Project.Models.DTOs
{
    public class ChangePwDTO
    {
        public string Token { get; set; }
        public string OldPassword { get; set; }
        public string Username { get; set; }
        public string NewPassword { get; set; }
    }
}
