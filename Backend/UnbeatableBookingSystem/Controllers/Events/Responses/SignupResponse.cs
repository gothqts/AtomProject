namespace UnbeatableBookingSystem.Controllers.Events.Responses;

public class SignupResponse
{
    public required Guid EventId { get; set; }
    
    public required Guid EntryId { get; set; }
    
    public string? Phone { get; set; }
    
    public string? Fio { get; set; }
    
    public string? Email { get; set; }
    
    public required DateTime DateTime { get; set; }
    
    public required Dictionary<string, string> DynamicFieldsData { get; set; }
}