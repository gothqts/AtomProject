using UnbeatableBookingSystem.Controllers.Base.Responses;

namespace UnbeatableBookingSystem.Controllers.Authorization.Responses;

public class RegisterResponse : BaseStatusResponse
{
    public Guid? UserId { get; set; }
    
    public required string AccessToken { get; set; }
}