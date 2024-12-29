namespace UnbeatableBookingSystem.Controllers.EventSignupWindows.Requests;

public class UpdateSignupWindowRequest
{
    public required string Title { get; set; }
    
    public required string Date { get; set; }
    
    public required string Time { get; set; }
    
    public required int MaxVisitors { get; set; }
}