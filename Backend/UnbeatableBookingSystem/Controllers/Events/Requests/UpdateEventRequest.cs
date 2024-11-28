namespace UnbeatableBookingSystem.Controllers.Events.Requests;

public class UpdateEventRequest
{
    public required bool IsPublic { get; set; }
    
    public required string Title { get; set; }
    
    public required bool IsOnline { get; set; }
    
    public required DateTime DateStart { get; set; }
    
    public required DateTime DateEnd { get; set; }
    
    public string? City { get; set; } = null!;

    public string? Address { get; set; } = null!;
    
    public required bool IsSignupOpened { get; set; }

    public string Description { get; set; } = null!;
}