using UnbeatableBookingSystem.Controllers.Events.Responses;

namespace UnbeatableBookingSystem.Controllers.DynamicFields.Responses;

public class DynamicFieldsResponse
{
    public required FormFieldResponse[] Fields { get; set; }
}