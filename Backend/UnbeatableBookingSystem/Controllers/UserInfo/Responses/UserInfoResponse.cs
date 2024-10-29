using UnbeatableBookingSystem.Controllers.Base.Responses;

namespace UnbeatableBookingSystem.Controllers.UserInfo.Responses;

public class UserInfoResponse
{
    public required Guid Id { get; set; }
    
    public required string Fio { get; set; }
    
    public required string Description { get; set; }
    
    public required string Status { get; set; }
    
    public required string AvatarImage { get; set; }
    
    public required BasicEventInfo[] CreatedEvents { get; set; }
}