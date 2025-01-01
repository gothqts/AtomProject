namespace UnbeatableBookingSystem.Controllers.Events.Responses;

public class SignupWindowResponse
{
    public required Guid Id { get; set; }
    
    public string Title { get; set; } = null!;
    
    public required DateTime DateTime { get; set; }
    
    public required int MaxVisitors { get; set; }
    
    public required int TicketsLeft { get; set; }
}