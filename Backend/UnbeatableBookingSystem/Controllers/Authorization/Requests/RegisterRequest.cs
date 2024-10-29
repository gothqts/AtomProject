using System.ComponentModel.DataAnnotations;

namespace UnbeatableBookingSystem.Controllers.Authorization.Requests;

public class RegisterRequest
{
    [EmailAddress]
    public required string Email { get; set; }
    
    [DataType(DataType.Password)]
    public required string Password { get; set; }
    
    public required string FIO { get; set; }
    
    public string? City { get; set; }
    
    public string? Status { get; set; }
}