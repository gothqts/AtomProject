namespace UnbeatableBookingSystem.Controllers.DynamicFields.Requests;

public class UpdateFieldRequest
{
    public required string Title { get; set; }
    
    public required bool IsRequired { get; set; }
    
    public required Guid FieldTypeId { get; set; }

    public required int MaxSymbols { get; set; }

    public string? MinValue { get; set; }
    
    public string? MaxValue { get; set; }
}