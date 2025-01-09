using System.Linq.Expressions;
using Booking.Application.Services;
using Booking.Core;
using Booking.Core.DataQuery;
using Booking.Core.Entities;
using Microsoft.AspNetCore.Mvc;
using UnbeatableBookingSystem.Controllers.Base;
using UnbeatableBookingSystem.Controllers.Base.Responses;
using UnbeatableBookingSystem.Controllers.Events.Requests;
using UnbeatableBookingSystem.Controllers.Events.Responses;
using UnbeatableBookingSystem.Utility;

namespace UnbeatableBookingSystem.Controllers.Events;

[Route("/api/events")]
public class EventsController : Controller
{
    private readonly BaseService<UserEvent> _eventService;
    private readonly BaseService<EventSignupWindow> _eventSignupWindowService;
    private readonly BaseService<OrganizerContacts> _contactsService;
    private readonly BaseService<EventSignupForm> _eventFormService;
    private readonly BaseService<FormDynamicField> _formDynamicFieldsService;
    private readonly EventSignupService _eventSignupService;
    private readonly EventBannerImageService _eventImageService;
    private readonly BaseService<EventSignupEntry> _entriesService;

    public EventsController(BaseService<UserEvent> eventService, BaseService<EventSignupWindow> eventSignupWindowService,
        BaseService<OrganizerContacts> contactsService, BaseService<EventSignupForm> eventFormService,
        BaseService<FormDynamicField> formDynamicFieldsService, EventSignupService eventSignupService,
        EventBannerImageService eventImageService, BaseService<EventSignupEntry> entriesService)
    {
        _eventService = eventService;
        _eventSignupWindowService = eventSignupWindowService;
        _contactsService = contactsService;
        _eventFormService = eventFormService;
        _formDynamicFieldsService = formDynamicFieldsService;
        _eventSignupService = eventSignupService;
        _eventImageService = eventImageService;
        _entriesService = entriesService;
    }
    
    [HttpGet("upcoming/{count:int}")]
    [ProducesResponseType(typeof(UpcomingEventsResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseStatusResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetUpcomingEvents([FromRoute] int count)
    {
        var take = count is > 0 and < 11 ? count : 10;
        var events = await _eventService.GetAsync(new DataQueryParams<UserEvent>
        {
            Expression = e => e.IsSignupOpened,
            Paging = new PagingParams
            {
                Skip = 0,
                Take = take
            },
            Sorting = new SortingParams<UserEvent>
            {
                OrderBy = e => e.DateStart,
                Ascending = true
            },
            Filters = [e => e.IsPublic]
        });
        var res = new UpcomingEventsResponse
        {
            Events = events.Select(e => DtoConverter.ConvertEventToBasicInfo(e, 
                    _eventImageService.EventImagesRelativePath, _eventImageService.DefaultEventImageFilename, Request))
                .ToArray()
        };
        return Ok(res);
    }
    
    [HttpGet]
    [ProducesResponseType(typeof(UpcomingEventsResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseStatusResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetEvents([FromQuery] string? city, [FromQuery] string? date,
        [FromQuery] string? time, [FromQuery] bool? online, [FromQuery] int? skip, 
        [FromQuery] int? take, [FromQuery] string? search)
    {
        var filters = new List<Expression<Func<EventSignupWindow, bool>>>()
        {
            w => w.Event.IsSignupOpened,
            w => w.TicketsLeft > 0
        };
        if (!string.IsNullOrWhiteSpace(city))
        {
            filters.Add(e => e.Event.City == city);
        }
        if (!string.IsNullOrWhiteSpace(date))
        {
            if (DateOnly.TryParseExact(date, "dd-MM-yyyy", out var dateOnly))
            {
                filters.Add(w => w.Date == dateOnly);
            }
        }
        if (!string.IsNullOrWhiteSpace(time))
        {
            if (TimeOnly.TryParseExact(time, "HH-mm", out var timeOnly))
            {
                filters.Add(w => w.Time == timeOnly);
            }
        }
        if (!string.IsNullOrWhiteSpace(search))
        {
            filters.Add(w => w.Event.Title.ToLower().Contains(search.ToLower()));
        }
        if (online != null)
        {
            filters.Add(e => e.Event.IsOnline == online.Value);
        }
        
        var signupWindows = await _eventSignupWindowService.GetAsync(new DataQueryParams<EventSignupWindow>
        {
            Expression = w => w.Event.IsPublic,
            Sorting = new SortingParams<EventSignupWindow>
            {
                OrderBy = e => e.Date,
                Ascending = true
            },
            Filters = filters,
            IncludeParams = new IncludeParams<EventSignupWindow>
            {
                IncludeProperties = [w => w.Event]
            }
        });
        var events = signupWindows
            .Select(w => w.Event)
            .DistinctBy(e => e.Id)
            .OrderBy(e => e.DateStart)
            .Skip(0);
        if (skip != null)
        {
            events = events.Skip(skip.Value);
        }
        if (take != null)
        {
            events = events.Take(take.Value);
        }
        var res = new UpcomingEventsResponse
        {
            Events = events.Select(e => DtoConverter.ConvertEventToBasicInfo(e, 
                    _eventImageService.EventImagesRelativePath, _eventImageService.DefaultEventImageFilename, Request))
                .ToArray()
        };
        return Ok(res);
    }
    
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(FullEventInfoResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseStatusResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetEventFullInfo([FromRoute] Guid id)
    {
        var userEvent = await _eventService.GetByIdOrDefaultAsync(id);
        if (userEvent == null)
        {
            return CustomResults.FailedRequest("Мероприятия с указанным id не существует.");
        }
        
        var windows = await _eventSignupWindowService.GetAsync(new DataQueryParams<EventSignupWindow>
        {
            Expression = w => w.EventId == userEvent.Id,
            Paging = null,
            Sorting = new SortingParams<EventSignupWindow>
            {
                OrderBy = w => w.Date,
                ThenBy = w => w.Time,
                Ascending = true
            }
        });
        var foundContacts = await _contactsService.GetAsync(new DataQueryParams<OrganizerContacts>
        {
            Expression = c => c.EventId == userEvent.Id
        });
        var res = new FullEventInfoResponse
        {
            Id = userEvent.Id,
            IsPublic = userEvent.IsPublic,
            Title = userEvent.Title,
            BannerImageFilepath = DtoConverter.GetEventImageUrl(userEvent, _eventImageService.EventImagesRelativePath, 
                _eventImageService.DefaultEventImageFilename, Request),
            IsOnline = userEvent.IsOnline,
            IsSignupOpened = userEvent.IsSignupOpened,
            City = userEvent.City,
            Address = userEvent.Address,
            DateStart = userEvent.DateStart,
            DateEnd = userEvent.DateEnd,
            Description = userEvent.Description,
            OrganizerContacts = foundContacts.Select(DtoConverter.OrganizerContactsToResponse).ToArray(),
            SignupWindows = windows.Select(DtoConverter.SignupWindowToResponse)
                .ToArray()
        };
        return Ok(res);
    }
    
    [HttpGet("{id:guid}/form")]
    [ProducesResponseType(typeof(EventFormResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseStatusResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetEventSignupForm([FromRoute] Guid id)
    {
        var userEvent = (await _eventService.GetAsync(new DataQueryParams<UserEvent>
        {
            Expression = e => e.Id == id
        }))[0];
        var form = (await _eventFormService.GetAsync(new DataQueryParams<EventSignupForm>
        {
            Expression = form => form.EventId == userEvent.Id
        }))[0];
        var fields = await _formDynamicFieldsService.GetAsync(new DataQueryParams<FormDynamicField>
        {
            Expression = field => field.EventFormId == form.Id,
            IncludeParams = new IncludeParams<FormDynamicField>
            {
                IncludeProperties = [field => field.FieldType]
            }
        });
        
        var res = new EventFormResponse
        {
            IsFioRequired = form.IsFioRequired,
            IsPhoneRequired = form.IsPhoneRequired,
            IsEmailRequired = form.IsEmailRequired,
            DynamicFields = fields.Select(DtoConverter.DynamicFieldToResponse).ToArray()
        };
        return Ok(res);
    }
    
    [HttpPost("{id:guid}/sign-up")]
    [ProducesResponseType(typeof(SignupResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseStatusResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> SignupForEvent([FromRoute] Guid id, [FromBody] SignupRequest request)
    {
        var idClaim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == AuthOptions.ClaimTypeUserId);
        Guid? userId = idClaim == null ? null : new Guid(idClaim.Value);
        
        var window = await _eventSignupWindowService.GetByIdOrDefaultAsync(request.SignupWindowId);
        if (window == null)
        {
            return BadRequest(new BaseStatusResponse
            {
                Completed = false,
                Status = "Failed",
                Message = "Window with provided signupWindowId not found."
            });
        }
        var signupInfo = await _eventSignupService.SignupUserToEventAsync(userId, request.SignupWindowId,
            request.Phone, request.Email, request.Fio, request.DynamicFieldsValues);
        if (!signupInfo.Completed || signupInfo.Entry == null)
        {
            return BadRequest(new BaseStatusResponse
            {
                Completed = false,
                Status = "Failed",
                Message = signupInfo.Comment
            });
        }
        
        // TODO: send Email sign up success
        
        var entryInfo = await _eventSignupService.GetEntryFormValues(signupInfo.Entry.Id);
        
        var res = new SignupResponse
        {
            DynamicFieldsData = entryInfo.DynamicValues,
            EntryId = signupInfo.Entry.Id,
            Phone = entryInfo.Phone,
            Fio = entryInfo.Fio,
            Email = entryInfo.Email,
            DateTime = new DateTime(window.Date,
                window.Time,
                DateTimeKind.Utc),
            EventId = id
        };
        return Ok(res);
    }
    
    [HttpGet("{eventId:guid}/entries/{entryId:guid}")]
    [ProducesResponseType(typeof(SignupResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseStatusResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> SignupForEvent([FromRoute] Guid eventId, [FromRoute] Guid entryId)
    {
        var userEvent = await _eventService.GetByIdOrDefaultAsync(eventId);
        if (userEvent == null)
        {
            return BadRequest(new BaseStatusResponse
            {
                Completed = false,
                Status = "Failed",
                Message = "Event with provided eventId not found."
            });
        }
        var entries = await _entriesService.GetAsync(new DataQueryParams<EventSignupEntry>
        {
            Expression = e => e.Id == entryId,
            IncludeParams = new IncludeParams<EventSignupEntry>
            {
                IncludeProperties = [e => e.SignupWindow]
            }
        });
        if (entries.Length == 0)
        {
            return BadRequest(new BaseStatusResponse
            {
                Completed = false,
                Status = "Failed",
                Message = "Entry with provided entryId not found."
            });
        }

        var entry = entries.First();
        var entryInfo = await _eventSignupService.GetEntryFormValues(entryId);
        
        var res = new SignupResponse
        {
            DynamicFieldsData = entryInfo.DynamicValues,
            EntryId = entryId,
            Phone = entryInfo.Phone,
            Fio = entryInfo.Fio,
            Email = entryInfo.Email,
            DateTime = new DateTime(entry.SignupWindow.Date,
                entry.SignupWindow.Time,
                DateTimeKind.Utc),
            EventId = eventId
        };
        return Ok(res);
    }
}