namespace UnbeatableBookingSystem.Controllers.Admin.Responses;

public class RoleResponse
{
    public Guid Id { get; set; }
    
    public required string Title { get; set; }
    
    public required bool CanEditOthersEvents { get; set; }
    
    public required bool IsAdmin { get; set; }
}