using Booking.Application.Services;
using Booking.Core.DataQuery;
using Booking.Core.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UnbeatableBookingSystem.Controllers.Base;
using UnbeatableBookingSystem.Controllers.Base.Responses;
using UnbeatableBookingSystem.Controllers.Events.Responses;
using UnbeatableBookingSystem.Controllers.MyEvents.Requests;
using UnbeatableBookingSystem.Controllers.MyEvents.Responses;
using UnbeatableBookingSystem.Controllers.UserInfo.Responses;
using UnbeatableBookingSystem.Utility;

namespace UnbeatableBookingSystem.Controllers.MyEvents;

[Route("/api/my-events")]
[Authorize]
public class MyEventsController : Controller
{
    private readonly BaseService<UserEvent> _eventService;
    private readonly BaseService<EventSignupWindow> _eventSignupWindowService;
    private readonly BaseService<OrganizerContacts> _contactsService;
    private readonly BaseService<EventSignupForm> _eventFormService;
    private readonly BaseService<FormDynamicField> _formDynamicFieldsService;
    private readonly BaseService<EntryFieldValue> _entryFieldValueService;
    private readonly BaseService<EventSignupEntry> _entryService;
    private readonly ControllerUtils _controllerUtils;
    private readonly EventBannerImageService _eventImageService;
    private readonly IWebHostEnvironment _env;

    public MyEventsController(BaseService<UserEvent> eventService, BaseService<EventSignupWindow> eventSignupWindowService,
        BaseService<OrganizerContacts> contactsService, BaseService<EventSignupForm> eventFormService,
        BaseService<FormDynamicField> formDynamicFieldsService, BaseService<EntryFieldValue> entryFieldValueService,
        BaseService<EventSignupEntry> entryService, ControllerUtils controllerUtils, 
        EventBannerImageService eventImageService, IWebHostEnvironment env)
    {
        _eventService = eventService;
        _eventSignupWindowService = eventSignupWindowService;
        _contactsService = contactsService;
        _eventFormService = eventFormService;
        _formDynamicFieldsService = formDynamicFieldsService;
        _entryFieldValueService = entryFieldValueService;
        _entryService = entryService;
        _controllerUtils = controllerUtils;
        _eventImageService = eventImageService;
        _env = env;
    }
    
    [HttpGet]
    [ProducesResponseType(typeof(UpcomingEventsResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseStatusResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetUserEvents([FromQuery] int? skip, [FromQuery] int? take, [FromQuery] bool? finished)
    {
        var info = _controllerUtils.TryGetUserId(HttpContext);
        if (!info.Succes)
        {
            return CustomResults.FailedRequest(info.ErrorMsg);
        }
        var userId = info.UserId!.Value;
        var dataQueryParams = new DataQueryParams<UserEvent>
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
        };
        if (finished.HasValue && finished.Value)
        {
            dataQueryParams.Filters = [e => e.DateEnd < DateTime.Today.ToUniversalTime()];
        }
        else if (finished.HasValue && !finished.Value)
        {
            dataQueryParams.Filters = [e => e.DateEnd >= DateTime.Today.ToUniversalTime()];
        }
        var events = await _eventService.GetAsync(dataQueryParams);
        
        var res = new UpcomingEventsResponse
        {
            Events = events.Select(e => DtoConverter.ConvertEventToBasicInfo(e, 
                    _eventImageService.EventImagesRelativePath, _eventImageService.DefaultEventImageFilename, Request))
                .ToArray()
        };
        return Ok(res);
    }
    
    [HttpGet("{eventId:guid}")]
    [ProducesResponseType(typeof(EventResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseStatusResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetEvent([FromRoute] Guid eventId)
    {
        var info = _controllerUtils.TryGetUserId(HttpContext);
        if (!info.Succes)
        {
            return CustomResults.FailedRequest(info.ErrorMsg);
        }
        var userId = info.UserId!.Value;
        var userEvents = await _eventService.GetAsync(new DataQueryParams<UserEvent>
        {
            Expression = e => e.Id == eventId && e.CreatorUserId == userId
        });
        if (userEvents.Length == 0)
        {
            return CustomResults.FailedRequest("События с заданным id не существует, либо его создал другой пользователь.");
        }
        var userEvent = userEvents[0];
        var windows = await _eventSignupWindowService.GetAsync(new DataQueryParams<EventSignupWindow>
        {
            Expression = e => e.EventId == eventId
        });
        var forms = await _eventFormService.GetAsync(new DataQueryParams<EventSignupForm>
        {
            Expression = f => f.EventId == eventId
        });
        var form = forms[0];
        var fields = await _formDynamicFieldsService.GetAsync(new DataQueryParams<FormDynamicField>
        {
            Expression = field => field.EventFormId == form.Id,
            IncludeParams = new IncludeParams<FormDynamicField>
            {
                IncludeProperties = [f => f.FieldType]
            }
        });
        var contacts = await _contactsService.GetAsync(new DataQueryParams<OrganizerContacts>
        {
            Expression = c => c.EventId == eventId
        });
        var res = DtoConverter.CreateEventResponse(userEvent, windows, form, fields, contacts,
            _eventImageService.EventImagesRelativePath, _eventImageService.DefaultEventImageFilename, Request);
        return Ok(res);
    }
    
    [HttpPost]
    [ProducesResponseType(typeof(BasicEventInfoResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseStatusResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateUserEvent()
    {
        var info = _controllerUtils.TryGetUserId(HttpContext);
        if (!info.Succes)
        {
            return CustomResults.FailedRequest(info.ErrorMsg);
        }
        var userId = info.UserId!.Value;
        var userEvent = new UserEvent
        {
            Id = Guid.NewGuid(),
            CreationDate = DateTime.Now.ToUniversalTime(),
            CreatorUserId = userId,
            IsPublic = false,
            Title = "Новое мероприятие",
            BannerImageFilepath = Path.Combine(_eventImageService.EventImagesRelativePath, _eventImageService.DefaultEventImageFilename),
            IsOnline = true,
            IsSignupOpened = false,
            DateStart = DateTime.Now.ToUniversalTime(),
            DateEnd = DateTime.Now.ToUniversalTime(),
            Description = "Заполните описание нового мероприятия"
        };
        await _eventService.SaveAsync(userEvent);
        var form = new EventSignupForm
        {
            Id = Guid.NewGuid(),
            IsFioRequired = false,
            IsPhoneRequired = false,
            IsEmailRequired = false,
            EventId = userEvent.Id
        };
        await _eventFormService.SaveAsync(form);
        var res = DtoConverter.ConvertEventToBasicInfo(userEvent, 
            _eventImageService.EventImagesRelativePath, _eventImageService.DefaultEventImageFilename, Request);
        return Ok(res);
    }
    
    [HttpPut("{eventId:guid}")]
    [ProducesResponseType(typeof(BaseStatusResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseStatusResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateUserEvent([FromRoute] Guid eventId, 
        [FromBody] UpdateEventRequest request)
    {
        var info = await _controllerUtils.CheckUserCanEditEventAsync(HttpContext, eventId);
        if (!info.Success)
        {
            return CustomResults.FailedRequest(info.ErrorMsg);
        }
        var userEvent = (await _eventService.GetAsync(new DataQueryParams<UserEvent>
        {
            Expression = e => e.Id == eventId
        }))[0];
        userEvent.IsPublic = request.IsPublic;
        userEvent.Title = request.Title;
        userEvent.IsOnline = request.IsOnline;
        userEvent.IsSignupOpened = request.IsSignupOpened;
        userEvent.City = request.City;
        userEvent.Address = request.Address;
        userEvent.DateStart = request.DateStart;
        userEvent.DateEnd = request.DateEnd;
        userEvent.Description = request.Description;
        await _eventService.SaveAsync(userEvent);
        
        var res = new BaseStatusResponse
        {
            Completed = true,
            Status = "Успех",
            Message = "Информация о событии была обновлена."
        };
        return Ok(res);
    }
    
    [HttpDelete("{eventId:guid}")]
    [ProducesResponseType(typeof(BaseStatusResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseStatusResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteUserEvent([FromRoute] Guid eventId)
    {
        var info = await _controllerUtils.CheckUserCanEditEventAsync(HttpContext, eventId);
        if (!info.Success)
        {
            return CustomResults.FailedRequest(info.ErrorMsg);
        }
        var contacts = await _contactsService.GetAsync(new DataQueryParams<OrganizerContacts>
        {
            Expression = c => c.EventId == eventId
        });
        await _contactsService.RemoveRangeAsync(contacts);
        var windows = await _eventSignupWindowService.GetAsync(new DataQueryParams<EventSignupWindow>
        {
            Expression = w => w.EventId == eventId,
            Paging = null,
            Sorting = null,
            Filters = null,
            IncludeParams = null
        });
        foreach (var window in windows)
        {
            var entries = await _entryService.GetAsync(new DataQueryParams<EventSignupEntry>
            {
                Expression = e => e.SignupWindowId == window.Id
            });
            foreach (var entry in entries)
            {
                var fieldValues = await _entryFieldValueService.GetAsync(new DataQueryParams<EntryFieldValue>
                {
                    Expression = v => v.EventSignupEntryId == entry.Id
                });
                await _entryFieldValueService.RemoveRangeAsync(fieldValues);
            }

            await _entryService.RemoveRangeAsync(entries);
        }
        await _eventSignupWindowService.RemoveRangeAsync(windows);
        
        var form = (await _eventFormService.GetAsync(new DataQueryParams<EventSignupForm>
        {
            Expression = f => f.EventId == eventId
        }))[0];
        var fields = await _formDynamicFieldsService.GetAsync(new DataQueryParams<FormDynamicField>
        {
            Expression = f => f.EventFormId == form.Id
        });
        await _formDynamicFieldsService.RemoveRangeAsync(fields);
        await _eventFormService.TryRemoveAsync(form.Id);
        await _eventService.TryRemoveAsync(eventId);
        
        var res = new BaseStatusResponse
        {
            Completed = true,
            Status = "Успех",
            Message = "Вся информация о событии и записях была удалена."
        };
        return Ok(res);
    }
    
    [HttpPost("{eventId:guid}/image")]
    [Authorize]
    [ProducesResponseType(typeof(UpdateAvatarResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseStatusResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateEventImage([FromRoute] Guid eventId, IFormFile? file)
    {
        if (file == null)
        {
            return CustomResults.FailedRequest("Запрос не содержит файл.");
        }
        var info = await _controllerUtils.CheckUserCanEditEventAsync(HttpContext, eventId);
        if (!info.Success)
        {
            return CustomResults.FailedRequest(info.ErrorMsg);
        }

        var userEvent = await _eventService.GetByIdOrDefaultAsync(eventId);
        await _eventImageService.UpdateEventImageAsync(userEvent!, _env.WebRootPath, file);
        
        return Ok(new UpdateAvatarResponse
        {
            Status = "Успех",
            Message = "Изображение мероприятия было успешно обновлено.",
            Completed = true,
            Image = DtoConverter.GetEventImageUrl(userEvent!, _eventImageService.EventImagesRelativePath, 
                _eventImageService.DefaultEventImageFilename, Request)
        });
    }
    
    [HttpDelete("{eventId:guid}/image")]
    [Authorize]
    [ProducesResponseType(typeof(UpdateAvatarResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseStatusResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateEventImage([FromRoute] Guid eventId)
    {
        var info = await _controllerUtils.CheckUserCanEditEventAsync(HttpContext, eventId);
        if (!info.Success)
        {
            return CustomResults.FailedRequest(info.ErrorMsg);
        }

        var userEvent = await _eventService.GetByIdOrDefaultAsync(eventId);
        await _eventImageService.UpdateEventImageAsync(userEvent!, _env.WebRootPath, null);
        
        return Ok(new UpdateAvatarResponse
        {
            Status = "Успех",
            Message = "Изображение мероприятия было успешно обновлено.",
            Completed = true,
            Image = DtoConverter.GetEventImageUrl(userEvent!, _eventImageService.EventImagesRelativePath, 
                _eventImageService.DefaultEventImageFilename, Request)
        });
    }
}