using UnbeatableBookingSystem.Controllers.Base.Responses;

namespace UnbeatableBookingSystem.Controllers.Events.Responses;

public class UpcomingEventsResponse
{
    public required BasicEventInfoResponse[] Events { get; set; }
}