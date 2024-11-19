namespace UnbeatableBookingSystem.Controllers.UserActions.Responses;

public class SignupResponse
{
    public Guid EntryId { get; set; }
    
    public string? Phone { get; set; }
    
    public string? Fio { get; set; }
    
    public string? Email { get; set; }
    
    public required DateTime DateTime { get; set; }
    
    public required Dictionary<string, string> DynamicFieldsData { get; set; }
}