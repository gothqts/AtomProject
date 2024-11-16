using System.Net.Mail;

namespace Booking.Application.Services;

public class EmailService
{
    public SmtpConnectionSettings? ConnectionSettings { get; private set; }
    
    public void InitializeSettings(SmtpConnectionSettings settings)
    {
        ConnectionSettings = settings;
    }
    
    public async Task SendEmailAsync(string subject, string authorEmail, string body, bool isHtml, params string[] toEmails)
    {
        if (ConnectionSettings == null)
        {
            throw new InvalidOperationException($"Service must initialized with {nameof(InitializeSettings)}() method before sending messages.");
        }
        var client = new SmtpClient(ConnectionSettings.SmtpServer)
        {
            Credentials = ConnectionSettings.AuthCredentials,
            Port = ConnectionSettings.Port,
            EnableSsl = ConnectionSettings.EnableSsl
        };
        var mailMessage = new MailMessage
        {
            From = new MailAddress(authorEmail),
            Subject = subject,
            Body = body,
            IsBodyHtml = isHtml
        };
        foreach (var mail in toEmails)
        {
            mailMessage.To.Add(mail);
        }
        
        await client.SendMailAsync(mailMessage);
    }
}