using API_Project.Enums;
using System.Text.RegularExpressions;

namespace API_Project.Helpers
{
    public static class CheckAuth
    {
        public static EmailCheckResult CheckEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return EmailCheckResult.Empty;

            string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            return Regex.IsMatch(email, pattern, RegexOptions.IgnoreCase)
                ? EmailCheckResult.Valid
                : EmailCheckResult.InvalidFormat;
        }

        public static PasswordCheckResult CheckPassword(string pw)
        {
            if (pw.Length < 7)
                return PasswordCheckResult.TooShort;

            if (pw.Length > 16)
                return PasswordCheckResult.TooLong;

            if (!Regex.IsMatch(pw, "[A-Z]"))
                return PasswordCheckResult.MissingUppercase;

            if (!Regex.IsMatch(pw, "[a-z]"))
                return PasswordCheckResult.MissingLowercase;

            if (!Regex.IsMatch(pw, @"\d"))
                return PasswordCheckResult.MissingDigit;

            if (!Regex.IsMatch(pw, @"[!-/:-@\[-_{-~]"))
                return PasswordCheckResult.MissingSpecialChar;

            if (Regex.IsMatch(pw, @"[^\dA-Za-z!-/:-@\[-_{-~]"))
                return PasswordCheckResult.ContainsInvalidChar;

            return PasswordCheckResult.Valid;
        }

        public static PhoneCheckResult CheckPhoneNumber(string phone)
        {
            if (string.IsNullOrWhiteSpace(phone) || phone.Length != 10)
                return PhoneCheckResult.InvalidLength;

            if (phone[0] != '0')
                return PhoneCheckResult.NotStartWithZero;

            return PhoneCheckResult.Valid;
        }
    }
}
