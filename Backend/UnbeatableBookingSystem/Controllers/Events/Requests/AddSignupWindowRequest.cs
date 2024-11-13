namespace UnbeatableBookingSystem.Controllers.Events.Requests;

public class AddSignupWindowRequest
{
    public string Title { get; set; }
    
    public string Date { get; set; }
    
    public string Time { get; set; }
    
    public int MaxVisitors { get; set; }
    
    public int AlreadyOccupiedPlaces { get; set; }
}