namespace UnbeatableBookingSystem.Controllers.Events.Requests;

public class SignupRequest
{
    public required Guid SignupWindowId { get; set; }
    
    public string? Phone { get; set; }
    
    public string? Email { get; set; }

    public string? Fio { get; set; }

    public required Dictionary<Guid, string> DynamicFieldsValues { get; set; }
}