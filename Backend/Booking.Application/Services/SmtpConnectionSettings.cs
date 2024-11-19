using System.Net;

namespace Booking.Application.Services;

public class SmtpConnectionSettings
{
    public required string SmtpServer { get; init; }
    
    public required NetworkCredential AuthCredentials { get; init; }
    
    public int Port { get; } = 587;
    
    public bool EnableSsl { get; } = true;
}