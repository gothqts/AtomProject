namespace UnbeatableBookingSystem.Controllers.Base.Responses;

public class BasicEventInfoResponse
{
    public required Guid Id { get; set; }
    
    public required bool IsPublic { get; set; }
    
    public required string Title { get; set; }
    
    public required string BannerImage { get; set; }
    
    public required DateTime DateStart { get; set; }
    
    public required DateTime DateEnd { get; set; }
    
    public required bool IsOnline { get; set; }
    
    public string? City { get; set; }
    
    public string? Address { get; set; }
    
    public required bool IsSignupOpened { get; set; }

    public string Description { get; set; } = null!;
}