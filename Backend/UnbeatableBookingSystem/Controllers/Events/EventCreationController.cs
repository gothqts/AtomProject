using System.Security.Claims;
using Booking.Application.Services;
using Booking.Core.DataQuery;
using Booking.Core.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UnbeatableBookingSystem.Controllers.Base.Responses;
using UnbeatableBookingSystem.Controllers.Events.Requests;
using UnbeatableBookingSystem.Controllers.UserActions.Responses;
using UnbeatableBookingSystem.Utility;

namespace UnbeatableBookingSystem.Controllers.Events;

[Route("/api/my-events")]
[Authorize]
public class EventCreationController : Controller
{
    private readonly BaseService<UserEvent> _eventService;
    private readonly BaseService<EventSignupWindow> _eventSignupWindowService;
    private readonly BaseService<OrganizerContacts> _contactsService;
    private readonly BaseService<EventSignupForm> _eventFormService;
    private readonly BaseService<FormDynamicField> _formDynamicFieldsService;
    private readonly EventSignupService _eventSignupService;
    private readonly BaseService<User> _userService;
    private readonly BaseService<UserRole> _roleService;
    private readonly BaseService<EntryFieldValue> _entryFieldValueService;
    private readonly BaseService<EventSignupEntry> _entryService;

    public EventCreationController(BaseService<UserEvent> eventService, BaseService<EventSignupWindow> eventSignupWindowService,
        BaseService<OrganizerContacts> contactsService, BaseService<EventSignupForm> eventFormService,
        BaseService<FormDynamicField> formDynamicFieldsService, EventSignupService eventSignupService,
        BaseService<User> userService, BaseService<UserRole> roleService, BaseService<EntryFieldValue> entryFieldValueService,
        BaseService<EventSignupEntry> entryService)
    {
        _eventService = eventService;
        _eventSignupWindowService = eventSignupWindowService;
        _contactsService = contactsService;
        _eventFormService = eventFormService;
        _formDynamicFieldsService = formDynamicFieldsService;
        _eventSignupService = eventSignupService;
        _userService = userService;
        _roleService = roleService;
        _entryFieldValueService = entryFieldValueService;
        _entryService = entryService;
    }
    
    [HttpGet]
    [ProducesResponseType(typeof(UpcomingEventsResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseStatusResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetUserEvents([FromQuery] int? skip, [FromQuery] int? take)
    {
        var info = await TryGetUserIdAsync();
        if (!info.Succes)
        {
            return FailedRequest(info.ErrorMsg);
        }
        var userId = info.UserId!.Value;
        var events = await _eventService.GetAsync(new DataQueryParams<UserEvent>
        {
            Expression = e => e.CreatorUserId == userId,
            Paging = new PagingParams
            {
                Skip = skip is > 0 ? skip.Value : 0,
                Take = take is > 0 ? take.Value : 10
            },
            Sorting = new SortingParams<UserEvent>
            {
                OrderBy = e => e.CreationDate,
                Ascending = false
            }
        });
        
        var res = new UpcomingEventsResponse
        {
            Events = events.Select(DtoConverter.ConvertEventToBasicInfo).ToArray()
        };
        return Ok(res);
    }
    
    [HttpPost]
    [ProducesResponseType(typeof(BasicEventInfoResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseStatusResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateUserEvent()
    {
        var info = await TryGetUserIdAsync();
        if (!info.Succes)
        {
            return FailedRequest(info.ErrorMsg);
        }
        var userId = info.UserId!.Value;
        var userEvent = new UserEvent
        {
            Id = Guid.NewGuid(),
            CreationDate = DateTime.Now.ToUniversalTime(),
            CreatorUserId = userId,
            IsPublic = false,
            Title = "Новое мероприятие",
            BannerImageFilepath = null,
            IsOnline = true,
            IsSignupOpened = false,
            DateStart = DateTime.Now.ToUniversalTime(),
            DateEnd = DateTime.Now.ToUniversalTime(),
            Description = "Заполните описание нового события"
        };
        await _eventService.SaveAsync(userEvent);
        var res = DtoConverter.ConvertEventToBasicInfo(userEvent);
        return Ok(res);
    }
    
    [HttpPost("{eventId:guid}/windows")]
    [ProducesResponseType(typeof(SignupWindowResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseStatusResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateSignupWindowForUserEvent([FromRoute] Guid eventId, 
        [FromBody] AddSignupWindowRequest request)
    {
        var check = await CheckUserCanEditEventAsync(eventId);
        if (!check.Success)
        {
            return FailedRequest(check.ErrorMsg);
        }

        var window = new EventSignupWindow
        {
            Id = Guid.NewGuid(),
            EventId = eventId,
            Title = request.Title,
            Date = DateOnly.ParseExact(request.Date, "dd-MM-yyyy"),
            Time = TimeOnly.ParseExact(request.Time, "HH-mm"),
            MaxVisitors = request.MaxVisitors,
            TicketsLeft = request.MaxVisitors - request.AlreadyOccupiedPlaces
        };
        if (window.TicketsLeft < 0)
        {
            window.TicketsLeft = 0;
        }

        await _eventSignupWindowService.SaveAsync(window);
        var res = DtoConverter.SignupWindowToResponse(window);
        return Ok(res);
    }
    
    [HttpDelete("{eventId:guid}/windows/{windowId:guid}")]
    [ProducesResponseType(typeof(BaseStatusResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseStatusResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> RemoveSignupWindowFromUserEvent([FromRoute] Guid eventId, [FromRoute] Guid windowId)
    {
        var check = await CheckUserCanEditEventAsync(eventId);
        if (!check.Success)
        {
            return FailedRequest(check.ErrorMsg);
        }

        var window = await _eventSignupWindowService.GetByIdOrDefaultAsync(windowId);
        if (window == null)
        {
            return FailedRequest("No window with that Id was found.");
        }

        var entries = await _entryService.GetAsync(new DataQueryParams<EventSignupEntry>
        {
            Expression = entry => entry.SignupWindowId == windowId
        });
        // TODO Добавить оповещения на почту пользователей, чьи записи были отменены?
        foreach (var entry in entries)
        {
            var fieldValues = await _entryFieldValueService.GetAsync(new DataQueryParams<EntryFieldValue>
            {
                Expression = fieldValue => fieldValue.EventSignupEntryId == entry.Id
            });
            await _entryFieldValueService.RemoveRangeAsync(fieldValues);
        }

        await _entryService.RemoveRangeAsync(entries);
        
        await _eventSignupWindowService.TryRemoveAsync(window.Id);
        return Ok(new BaseStatusResponse
        {
            Completed = true,
            Status = "Removed",
            Message = "Window has been successfully deleted."
        });
    }
    
    [HttpPut("{eventId:guid}/windows/{windowId:guid}")]
    [ProducesResponseType(typeof(SignupWindowResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseStatusResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> EditSignupWindowFromUserEvent([FromRoute] Guid eventId, [FromRoute] Guid windowId,
        [FromBody] UpdateSignupWindowRequest request)
    {
        var check = await CheckUserCanEditEventAsync(eventId);
        if (!check.Success)
        {
            return FailedRequest(check.ErrorMsg);
        }

        var window = await _eventSignupWindowService.GetByIdOrDefaultAsync(windowId);
        if (window == null)
        {
            return FailedRequest("No window with that Id was found.");
        }
        if (window.Date != request.Date || window.Time != request.Time)
        {
            // TODO Добавить оповещения на почту пользователей, чьи записи были изменены?
        }
        window.Title = request.Title;
        window.Date = request.Date;
        window.Time = request.Time;
        var alreadyOccupied = window.MaxVisitors - window.TicketsLeft;
        window.MaxVisitors = request.MaxVisitors < alreadyOccupied ? alreadyOccupied : request.MaxVisitors;
        
        await _eventSignupWindowService.SaveAsync(window);
        
        return Ok(DtoConverter.SignupWindowToResponse(window));
    }
    
    private BadRequestObjectResult FailedRequest(string msg)
    {
        return BadRequest(new BaseStatusResponse
        {
            Status = "Failed",
            Message = msg,
            Completed = false
        });
    }

    private async Task<(bool Success, string ErrorMsg, User? User)> CheckUserCanEditEventAsync(Guid eventId)
    {
        var info = await TryGetSelfUserAsync(true);
        if (!info.Succes)
        {
            return (false, info.ErrorMsg, null);
        }
        var user = info.User!;
        var userEvent = (await _eventService.GetAsync(new DataQueryParams<UserEvent>
        {
            Expression = e => e.Id == eventId
        }))[0];
        if (userEvent.CreatorUserId != user.Id || !(user.Role.CanEditOthersEvents || user.Role.IsAdmin))
        {
            return (false, "You cannot edit this event.", null);
        }

        return (true, "", user);
    }
    
    private async Task<(bool Succes, User? User, string ErrorMsg)> TryGetSelfUserAsync(bool includeRole = false)
    {
        var idClaim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
        if (idClaim == null)
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return (false, null, "User doesn't have proper claims, unauthorized.");
        }
        var user = await _userService.GetByIdOrDefaultAsync(new Guid(idClaim.Value));
        if (user == null)
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return (false, null, "Operation failed. No user with that id was found. Unauthorized.");
        }

        if (includeRole)
        {
            var role = await _roleService.GetByIdOrDefaultAsync(user.RoleId);
            user.Role = role;
        }
        
        return (true, user, "");
    }
    
    private async Task<(bool Succes, Guid? UserId, string ErrorMsg)> TryGetUserIdAsync()
    {
        var idClaim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
        if (idClaim == null)
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return (false, null, "User doesn't have proper claims, unauthorized.");
        }
        
        return (true, new Guid(idClaim.Value), "");
    }
}