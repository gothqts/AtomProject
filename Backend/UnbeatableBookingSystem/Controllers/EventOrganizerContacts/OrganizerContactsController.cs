using Booking.Application.Services;
using Booking.Core.DataQuery;
using Booking.Core.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UnbeatableBookingSystem.Controllers.Base;
using UnbeatableBookingSystem.Controllers.Base.Responses;
using UnbeatableBookingSystem.Controllers.EventOrganizerContacts.Requests;
using UnbeatableBookingSystem.Controllers.EventOrganizerContacts.Responses;
using UnbeatableBookingSystem.Controllers.UserActions.Responses;
using UnbeatableBookingSystem.Utility;

namespace UnbeatableBookingSystem.Controllers.EventOrganizerContacts;

[Route("/api/my-events/{eventId:guid}/contacts")]
[Authorize]
public class OrganizerContactsController : Controller
{
    private readonly BaseService<OrganizerContacts> _contactsService;
    private readonly ControllerUtils _controllerUtils;

    public OrganizerContactsController(BaseService<OrganizerContacts> contactsService, ControllerUtils controllerUtils)
    {
        _contactsService = contactsService;
        _controllerUtils = controllerUtils;
    }
    
    [HttpGet]
    [ProducesResponseType(typeof(EventContactsListResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseStatusResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetEventContacts([FromRoute] Guid eventId)
    {
        var check = await _controllerUtils.CheckUserCanEditEventAsync(HttpContext, eventId);
        if (!check.Success)
        {
            return CustomResults.FailedRequest(check.ErrorMsg);
        }

        var contacts = await _contactsService.GetAsync(new DataQueryParams<OrganizerContacts>
        {
            Expression = c => c.EventId == eventId
        });
        var res = new EventContactsListResponse
        {
            Contacts = contacts.Select(DtoConverter.OrganizerContactsToResponse).ToArray()
        };
        return Ok(res);
    }
    
    [HttpPost]
    [ProducesResponseType(typeof(ContactsResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseStatusResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateEventContacts([FromRoute] Guid eventId)
    {
        var check = await _controllerUtils.CheckUserCanEditEventAsync(HttpContext, eventId);
        if (!check.Success)
        {
            return CustomResults.FailedRequest(check.ErrorMsg);
        }
        var contacts = new OrganizerContacts
        {
            Id = Guid.NewGuid(),
            EventId = eventId,
            Email = "example@email.com",
            Fio = "",
            Phone = "",
            Telegram = ""
        };
        await _contactsService.SaveAsync(contacts);
        
        var res = DtoConverter.OrganizerContactsToResponse(contacts);
        return Ok(res);
    }
    
    [HttpPut("{contactsId:guid}")]
    [ProducesResponseType(typeof(ContactsResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseStatusResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateEventContacts([FromRoute] Guid eventId, [FromRoute] Guid contactsId,
        [FromBody] UpdateOrganizerContactsRequest request)
    {
        var check = await _controllerUtils.CheckUserCanEditEventAsync(HttpContext, eventId);
        if (!check.Success)
        {
            return CustomResults.FailedRequest(check.ErrorMsg);
        }

        var contacts = await _contactsService.GetAsync(new DataQueryParams<OrganizerContacts>
        {
            Expression = c => c.Id == contactsId
        });
        if (contacts.Length != 1)
        {
            return CustomResults.FailedRequest("Контактов с указанным id не было найдено");
        }
        var contact = contacts[0];
        contact.Email = request.Email;
        contact.Fio = request.Fio;
        contact.Phone = request.Phone;
        contact.Telegram = request.Telegram;
        
        await _contactsService.SaveAsync(contact);
        var res = DtoConverter.OrganizerContactsToResponse(contact);
        return Ok(res);
    }
    
    [HttpDelete("{contactsId:guid}")]
    [ProducesResponseType(typeof(BaseStatusResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseStatusResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateSignupWindowForUserEvent([FromRoute] Guid eventId, [FromRoute] Guid contactsId)
    {
        var check = await _controllerUtils.CheckUserCanEditEventAsync(HttpContext, eventId);
        if (!check.Success)
        {
            return CustomResults.FailedRequest(check.ErrorMsg);
        }

        var contacts = await _contactsService.GetAsync(new DataQueryParams<OrganizerContacts>
        {
            Expression = c => c.Id == contactsId
        });
        if (contacts.Length != 1)
        {
            return CustomResults.FailedRequest("Контактов с указанным id не было найдено");
        }
        
        await _contactsService.TryRemoveAsync(contactsId);
        var res = new BaseStatusResponse
        {
            Completed = true,
            Status = "Успех",
            Message = "Контакты организатора были удалены."
        };
        return Ok(res);
    }
}