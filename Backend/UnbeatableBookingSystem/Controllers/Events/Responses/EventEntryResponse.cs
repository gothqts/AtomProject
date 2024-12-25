using UnbeatableBookingSystem.Controllers.UserInfo.Responses;

namespace UnbeatableBookingSystem.Controllers.Events.Responses;

public class EventEntryResponse
{
    public Guid EntryId { get; set; }
    
    public BriefUserInfoResponse? UserInfo { get; set; }
    
    public string? Phone { get; set; }
    
    public string? Fio { get; set; }
    
    public string? Email { get; set; }
    
    public required DateTime DateTime { get; set; }
    
    public required Dictionary<string, string> DynamicFieldsData { get; set; }
}