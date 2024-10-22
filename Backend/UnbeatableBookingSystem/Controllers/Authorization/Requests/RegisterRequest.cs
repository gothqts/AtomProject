using System.ComponentModel.DataAnnotations;

namespace UnbeatableBookingSystem.Controllers.Authorization.Requests;

public class RegisterRequest
{
    [EmailAddress]
    public required string Email { get; set; }
    
    [DataType(DataType.Password)]
    public required string Password { get; set; }
    
    public required string FIO { get; set; }
    
    [DataType(DataType.PhoneNumber)]
    public required string Phone { get; set; }
}