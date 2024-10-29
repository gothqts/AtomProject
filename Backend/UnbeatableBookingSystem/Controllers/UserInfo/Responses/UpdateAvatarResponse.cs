using UnbeatableBookingSystem.Controllers.Base.Responses;

namespace UnbeatableBookingSystem.Controllers.UserInfo.Responses;

public class UpdateAvatarResponse : BaseStatusResponse
{
    public required string Image { get; set; }
}