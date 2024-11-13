namespace UnbeatableBookingSystem.Controllers.UserActions.Responses;

public class FormFieldResponse
{
    public required Guid Id { get; set; }
    
    public required string Title { get; set; }
    
    public required bool IsRequired { get; set; }
    
    public string? Type { get; set; }
    
    public int MaxSymbols { get; set; } = -1;
    
    public string MinValue { get; set; } = null!;
    
    public string MaxValue { get; set; } = null!;
}