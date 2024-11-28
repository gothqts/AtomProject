namespace UnbeatableBookingSystem.Controllers.UserActions.Responses;

public class FormFieldResponse
{
    public required Guid Id { get; set; }
    
    public required string Title { get; set; }
    
    public required bool IsRequired { get; set; }
    
    public string? Type { get; set; }
    
    public int? MaxSymbols { get; set; }
    
    public string? MinValue { get; set; }
    
    public string? MaxValue { get; set; }
}