namespace UnbeatableBookingSystem.Controllers.UserInfo.Requests;

public class UpdateUserInfoRequest
{
    public required string Fio { get; set; }
    
    public required string Description { get; set; }
    
    public required string Status { get; set; }
    
    public required string City { get; set; }
}