namespace UnbeatableBookingSystem.Controllers.EventSignupWindows.Requests;

public class UpdateSignupWindowRequest
{
    public required string Title { get; set; }
    
    public required DateOnly Date { get; set; }
    
    public required TimeOnly Time { get; set; }
    
    public required int MaxVisitors { get; set; }
}