namespace UnbeatableBookingSystem.Controllers.Events.Requests;

public class AddSignupWindowRequest
{
    public required string Title { get; set; }
    
    public required string Date { get; set; }
    
    public required string Time { get; set; }
    
    public int MaxVisitors { get; set; }
    
    public int AlreadyOccupiedPlaces { get; set; }
}