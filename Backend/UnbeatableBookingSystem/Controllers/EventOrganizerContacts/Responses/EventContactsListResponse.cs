using UnbeatableBookingSystem.Controllers.UserActions.Responses;

namespace UnbeatableBookingSystem.Controllers.EventOrganizerContacts.Responses;

public class EventContactsListResponse
{
    public required ContactsResponse[] Contacts { get; set; }
}