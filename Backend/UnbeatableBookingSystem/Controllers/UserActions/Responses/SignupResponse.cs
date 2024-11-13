using UnbeatableBookingSystem.Controllers.Base.Responses;

namespace UnbeatableBookingSystem.Controllers.UserActions.Responses;

public class SignupResponse
{
    public Guid EntryId { get; set; }
    
    public string? Phone { get; set; } = null!;
    
    public string? Fio { get; set; } = null!;
    
    public string? Email { get; set; } = null!;
    
    public required DateTime DateTime { get; set; }
    
    public required Dictionary<string, string> DynamicFieldsData { get; set; }
}