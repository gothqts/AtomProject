namespace UnbeatableBookingSystem.Controllers.EventForm.Requests;

public class UpdateSignupFormRequest
{
    public required bool IsFioRequired { get; set; }
    
    public required bool IsPhoneRequired { get; set; }
    
    public required bool IsEmailRequired { get; set; }
}