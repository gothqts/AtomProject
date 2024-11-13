namespace UnbeatableBookingSystem.Controllers.UserActions.Responses;

public class EventFormResponse
{
    public required bool IsFioRequired { get; set; }
    
    public required bool IsPhoneRequired { get; set; }
    
    public required bool IsEmailRequired { get; set; }
    
    public required FormFieldResponse[] DynamicFields { get; set; }
}