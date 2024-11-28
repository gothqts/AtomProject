namespace UnbeatableBookingSystem.Controllers.EventSignupWindows.Requests;

public class CreateSignupWindowRequest
{
    public required string Title { get; set; }
    
    public required string Date { get; set; }
    
    public required string Time { get; set; }
    
    public int MaxVisitors { get; set; }
    
    public int AlreadyOccupiedPlaces { get; set; }
}