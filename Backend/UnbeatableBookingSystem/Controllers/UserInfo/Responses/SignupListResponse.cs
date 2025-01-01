using UnbeatableBookingSystem.Controllers.Events.Responses;

namespace UnbeatableBookingSystem.Controllers.UserInfo.Responses;

public class SignupListResponse
{
    public required SignupResponse[] Entries { get; set; }
}