namespace API_Project.Models.DTOs
{
    public class ChangePwDTO
    {
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }
    public class CheckOtpDTO
    {
        public string Username { get; set; }
        public string OTP { get; set; }
    }

}
