using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace UnbeatableBookingSystem.Controllers.Events;

[Route("/api/events")]
[Authorize]
public class EventCreationController : Controller
{
    
}