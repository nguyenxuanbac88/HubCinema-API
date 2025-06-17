
using System.Net;
using System.Net.Mail;
using API_Project.Models;
using Microsoft.Extensions.Options;


public class EmailService
{
    private readonly EmailSettings _emailSettings;
    public EmailService() { }
    public EmailService(IOptions<EmailSettings> emailSettings)
    {
        _emailSettings = emailSettings.Value;
    }

    public bool SendEmail(string toEmail, string subject, string body)
    {
        try
        {
            var mail = new MailMessage();
            mail.From = new MailAddress(_emailSettings.SenderEmail, _emailSettings.SenderName);
            mail.To.Add(toEmail);
            mail.Subject = subject;
            mail.Body = body;
            mail.IsBodyHtml = true;

            using (var smtp = new SmtpClient(_emailSettings.SmtpServer, _emailSettings.Port))
            {
                smtp.Credentials = new NetworkCredential(_emailSettings.Username, _emailSettings.Password);
                smtp.EnableSsl = _emailSettings.EnableSsl;
                smtp.Send(mail);
            }

            return true;
        }
        catch (Exception ex)
        {
            // Ghi log hoặc xử lý lỗi ở đây nếu cần
            Console.WriteLine("Lỗi gửi email: " + ex.Message);
            return false;
        }
    }
}
