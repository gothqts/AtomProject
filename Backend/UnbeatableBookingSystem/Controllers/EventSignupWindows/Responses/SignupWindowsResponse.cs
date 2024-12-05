using UnbeatableBookingSystem.Controllers.UserActions.Responses;

namespace UnbeatableBookingSystem.Controllers.EventSignupWindows.Responses;

public class SignupWindowsResponse
{
    public required SignupWindowResponse[] SignupWindows { get; set; }
}