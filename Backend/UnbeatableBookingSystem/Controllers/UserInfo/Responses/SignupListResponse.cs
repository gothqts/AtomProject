using UnbeatableBookingSystem.Controllers.UserActions.Responses;

namespace UnbeatableBookingSystem.Controllers.UserInfo.Responses;

public class SignupListResponse
{
    public required SignupResponse[] Entries { get; set; }
}