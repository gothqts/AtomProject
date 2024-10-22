using UnbeatableBookingSystem.Controllers.Base.Responses;

namespace UnbeatableBookingSystem.Controllers.Authorization.Responses;

public class LoginResponse : BaseStatusResponse
{
    public required Guid? UserId { get; set; }
}