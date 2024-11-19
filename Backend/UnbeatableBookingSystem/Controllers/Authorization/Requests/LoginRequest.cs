using System.ComponentModel.DataAnnotations;

namespace UnbeatableBookingSystem.Controllers.Authorization.Requests;

public class LoginRequest
{
    [EmailAddress]
    public required string Email { get; set; }
    
    [DataType(DataType.Password)]
    public required string Password { get; set; }
}