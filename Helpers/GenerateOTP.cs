namespace API_Project.Helpers
{
    public class GenerateOTP
    {
        public static string GenerateUserOTP()
        {
            var random = new Random();
            return random.Next(0, 1000000).ToString("D6"); 
        }

    }
}
