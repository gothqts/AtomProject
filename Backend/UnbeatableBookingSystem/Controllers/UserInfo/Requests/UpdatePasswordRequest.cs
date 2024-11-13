namespace UnbeatableBookingSystem.Controllers.UserInfo.Requests;

public class UpdatePasswordRequest
{
    public required string CurrentPassword { get; set; }
    
    public required string NewPassword { get; set; }
}