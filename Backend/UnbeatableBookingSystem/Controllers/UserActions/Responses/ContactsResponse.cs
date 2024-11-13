namespace UnbeatableBookingSystem.Controllers.UserActions.Responses;

public class ContactsResponse
{
    public required string Email { get; set; }
    
    public string Fio { get; set; } = null!;

    public string Phone { get; set; } = null!;
    
    public string Telegram { get; set; } = null!;
}