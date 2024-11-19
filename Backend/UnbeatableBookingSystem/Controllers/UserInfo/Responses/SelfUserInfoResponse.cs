using System.ComponentModel.DataAnnotations;
using UnbeatableBookingSystem.Controllers.Base.Responses;

namespace UnbeatableBookingSystem.Controllers.UserInfo.Responses;

public class SelfUserInfoResponse
{
    public required Guid Id { get; set; }
    
    [Phone]
    public required string Phone { get; set; }
    
    [EmailAddress]
    public required string Email { get; set; }
    
    public required string Fio { get; set; }
    
    public required string RoleTitle { get; set; }
    
    public required string Description { get; set; }
    
    public required string Status { get; set; }
    
    public required string AvatarImage { get; set; }
    
    public required BasicEventInfoResponse[] CreatedEvents { get; set; }
}