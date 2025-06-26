namespace API_Project.Models.DTOs
{

    public class ChangeEmailRequestDTO
    {
        public string EmailNew { get; set; }
    }

    public class ChangeEmailConfirmDTO
    {
        public string OtpToken { get; set; }
        public string Otp { get; set; }
    }

}
