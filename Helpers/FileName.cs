using System.Globalization;
using System.Text.RegularExpressions;
using System.Text;

namespace API_Project.Helpers
{
    public class FileName
    {
        public static string ToSafeFileName(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return "";

            string normalized = input.Normalize(NormalizationForm.FormD);
            var sb = new StringBuilder();

            foreach (var c in normalized)
            {
                var uc = CharUnicodeInfo.GetUnicodeCategory(c);
                if (uc != UnicodeCategory.NonSpacingMark)
                    sb.Append(c);
            }

            string noDiacritics = sb.ToString().Normalize(NormalizationForm.FormC);

            // Loại bỏ ký tự không hợp lệ + thay khoảng trắng bằng _
            string safe = Regex.Replace(noDiacritics, @"[^a-zA-Z0-9]", "_");
            safe = Regex.Replace(safe, "_{2,}", "_"); // gộp nhiều dấu _ lại
            return safe.Trim('_');
        }

    }
}
