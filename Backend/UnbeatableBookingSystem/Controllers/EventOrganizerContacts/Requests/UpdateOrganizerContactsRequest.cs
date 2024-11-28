namespace UnbeatableBookingSystem.Controllers.EventOrganizerContacts.Requests;

public class UpdateOrganizerContactsRequest
{
    public required string Email { get; set; }
    
    public string Fio { get; set; } = null!;

    public string Phone { get; set; } = null!;
    
    public string Telegram { get; set; } = null!;
}