using System.ComponentModel.DataAnnotations;

namespace UnbeatableBookingSystem.Controllers.UserInfo.Requests;

public class UpdatePhoneRequest
{
    [Phone]
    public required string Phone { get; set; }
}