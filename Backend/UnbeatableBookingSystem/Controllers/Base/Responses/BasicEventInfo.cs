namespace UnbeatableBookingSystem.Controllers.Base.Responses;

public class BasicEventInfo
{
    public required Guid Id { get; set; }
    
    public required string Title { get; set; }
    
    public required string BannerImage { get; set; }
    
    public required DateTime DateStart { get; set; }
    
    public required DateTime DateEnd { get; set; }
    
    public required bool IsOnline { get; set; }
    
    public required string City { get; set; }
    
    public required string Address { get; set; }
    
    public required bool IsSignupOpen { get; set; }
}