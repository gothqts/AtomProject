﻿using UnbeatableBookingSystem.Controllers.Base.Responses;

namespace UnbeatableBookingSystem.Controllers.UserActions.Responses;

public class UpcomingEventsResponse
{
    public BasicEventInfoResponse[] Events { get; set; }
}