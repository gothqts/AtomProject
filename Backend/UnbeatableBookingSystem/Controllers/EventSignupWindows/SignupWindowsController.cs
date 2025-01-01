using Booking.Application.Services;
using Booking.Core.DataQuery;
using Booking.Core.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UnbeatableBookingSystem.Controllers.Base;
using UnbeatableBookingSystem.Controllers.Base.Responses;
using UnbeatableBookingSystem.Controllers.Events.Responses;
using UnbeatableBookingSystem.Controllers.EventSignupWindows.Requests;
using UnbeatableBookingSystem.Controllers.EventSignupWindows.Responses;
using UnbeatableBookingSystem.Utility;

namespace UnbeatableBookingSystem.Controllers.EventSignupWindows;

[Route("/api/my-events/{eventId:guid}/windows")]
[Authorize]
public class SignupWindowsController : Controller
{
    private readonly ControllerUtils _controllerUtils;
    private readonly BaseService<EventSignupWindow> _eventSignupWindowService;
    private readonly BaseService<EventSignupEntry> _entryService;
    private readonly BaseService<EntryFieldValue> _entryFieldValueService;

    public SignupWindowsController(ControllerUtils controllerUtils, 
        BaseService<EventSignupWindow> eventSignupWindowService, BaseService<EventSignupEntry> entryService, 
        BaseService<EntryFieldValue> entryFieldValueService)
    {
        _controllerUtils = controllerUtils;
        _eventSignupWindowService = eventSignupWindowService;
        _entryService = entryService;
        _entryFieldValueService = entryFieldValueService;
    }
    
    [HttpGet]
    [ProducesResponseType(typeof(SignupWindowsResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseStatusResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetSignupWindows([FromRoute] Guid eventId)
    {
        var check = await _controllerUtils.CheckUserCanEditEventAsync(HttpContext, eventId);
        if (!check.Success)
        {
            return CustomResults.FailedRequest(check.ErrorMsg);
        }
        
        var windows = await _eventSignupWindowService.GetAsync(new DataQueryParams<EventSignupWindow>
        {
            Expression = w => w.EventId == eventId
        });
        var res = new SignupWindowsResponse
        {
            SignupWindows = windows.Select(DtoConverter.SignupWindowToResponse).ToArray()
        };
        return Ok(res);
    }
    
    [HttpPost]
    [ProducesResponseType(typeof(SignupWindowResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseStatusResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateSignupWindow([FromRoute] Guid eventId, [FromBody] CreateSignupWindowRequest request)
    {
        var check = await _controllerUtils.CheckUserCanEditEventAsync(HttpContext, eventId);
        if (!check.Success)
        {
            return CustomResults.FailedRequest(check.ErrorMsg);
        }
        var window = new EventSignupWindow
        {
            Id = Guid.NewGuid(),
            EventId = eventId,
            Title = request.Title,
            Date = DateOnly.MinValue,
            Time = TimeOnly.MinValue,
            MaxVisitors = request.MaxVisitors,
            TicketsLeft = request.MaxVisitors - request.AlreadyOccupiedPlaces
        };
        
        try
        {
            window.Date = DateOnly.ParseExact(request.Date, "dd-MM-yyyy");
            window.Time = TimeOnly.ParseExact(request.Time, "HH-mm");
        }
        catch (FormatException e)
        {
            return CustomResults.FailedRequest(
                "Дата или время указаны в неверном формате. Правильный формат даты: \"dd-MM-yyyy\"; Правильный формат времени: \"HH-mm\"");
        }
        
        if (window.TicketsLeft < 0)
        {
            window.TicketsLeft = 0;
        }

        await _eventSignupWindowService.SaveAsync(window);
        var res = DtoConverter.SignupWindowToResponse(window);
        return Ok(res);
    }
    
    [HttpPut("{windowId:guid}")]
    [ProducesResponseType(typeof(SignupWindowResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseStatusResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> EditSignupWindow([FromRoute] Guid eventId, [FromRoute] Guid windowId,
        [FromBody] UpdateSignupWindowRequest request)
    {
        var check = await _controllerUtils.CheckUserCanEditEventAsync(HttpContext, eventId);
        if (!check.Success)
        {
            return CustomResults.FailedRequest(check.ErrorMsg);
        }

        DateOnly date;
        TimeOnly time;
        try
        {
            date = DateOnly.ParseExact(request.Date, "dd-MM-yyyy");
            time = TimeOnly.ParseExact(request.Time, "HH-mm");
        }
        catch (FormatException e)
        {
            return CustomResults.FailedRequest(
                "Дата или время указаны в неверном формате. Правильный формат даты: \"dd-MM-yyyy\"; Правильный формат времени: \"HH-mm\"");
        }
        
        var windows = await _eventSignupWindowService.GetAsync(new DataQueryParams<EventSignupWindow>
        {
            Expression = w => w.EventId == eventId && w.Id == windowId
        });
        if (windows.Length != 1)
        {
            return CustomResults.FailedRequest("Не было найдено ни одного окна записи с указанным id.");
        }
        
        var window = windows[0];
        if (window.Date != date || window.Time != time)
        {
            // TODO Добавить оповещения на почту пользователей, чьи записи были изменены?
        }
        window.Title = request.Title;
        window.Date = date;
        window.Time = time;
        var alreadyOccupied = window.MaxVisitors - window.TicketsLeft;
        if (request.MaxVisitors > window.MaxVisitors)
        {
            window.TicketsLeft += request.MaxVisitors - window.MaxVisitors;
        }
        window.MaxVisitors = request.MaxVisitors < alreadyOccupied ? alreadyOccupied : request.MaxVisitors;
        
        await _eventSignupWindowService.SaveAsync(window);
        
        return Ok(DtoConverter.SignupWindowToResponse(window));
    }
    
    [HttpDelete("{windowId:guid}")]
    [ProducesResponseType(typeof(BaseStatusResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseStatusResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> RemoveSignupWindowFromUserEvent([FromRoute] Guid eventId, [FromRoute] Guid windowId)
    {
        var check = await _controllerUtils.CheckUserCanEditEventAsync(HttpContext, eventId);
        if (!check.Success)
        {
            return CustomResults.FailedRequest(check.ErrorMsg);
        }

        var windows = await _eventSignupWindowService.GetAsync(new DataQueryParams<EventSignupWindow>
        {
            Expression = w => w.EventId == eventId && w.Id == windowId
        });
        if (windows.Length != 1)
        {
            return CustomResults.FailedRequest("Не было найдено ни одного окна записи с указанным id.");
        }
        var window = windows[0];
        
        var entries = await _entryService.GetAsync(new DataQueryParams<EventSignupEntry>
        {
            Expression = entry => entry.SignupWindowId == windowId
        });
        foreach (var entry in entries)
        {
            var fieldValues = await _entryFieldValueService.GetAsync(new DataQueryParams<EntryFieldValue>
            {
                Expression = fieldValue => fieldValue.EventSignupEntryId == entry.Id
            });
            await _entryFieldValueService.RemoveRangeAsync(fieldValues);
        }
        
        // TODO Добавить оповещения на почту пользователей, чьи записи были удалены?
        await _entryService.RemoveRangeAsync(entries);
        
        await _eventSignupWindowService.TryRemoveAsync(window.Id);
        return Ok(new BaseStatusResponse
        {
            Completed = true,
            Status = "Успех",
            Message = "Окно записи удалено."
        });
    }
}