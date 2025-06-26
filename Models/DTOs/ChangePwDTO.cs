namespace API_Project.Models.DTOs
{
    public class CheckOtpDTO
    {
        public string Username { get; set; }
        public string OTP { get; set; }
        public string OtpToken { get; set; }
    }
    public class ConfirmPwDTO
    {
        public string Username { get; set; }
        public string OTP { get; set; }
        public string NewPW { get; set; }
        public string OtpToken { get; set; }
    }


}
