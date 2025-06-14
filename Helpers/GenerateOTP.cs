namespace API_Project.Helpers
{
    public class GenerateOTP
    {
        public static string GenerateUserOTP()
        {
            var ms = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            var last4 = (ms % 10000).ToString("D4");

            var random = new Random();
            var rand4 = random.Next(0, 10000).ToString("D4");

            return last4 + rand4;
        }
    }
}
