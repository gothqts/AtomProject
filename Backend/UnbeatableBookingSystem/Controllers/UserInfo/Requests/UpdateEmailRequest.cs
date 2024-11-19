using System.ComponentModel.DataAnnotations;

namespace UnbeatableBookingSystem.Controllers.UserInfo.Requests;

public class UpdateEmailRequest
{
    [EmailAddress]
    public required string Email { get; set; }
}