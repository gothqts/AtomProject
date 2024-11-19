using System.ComponentModel.DataAnnotations;
using Booking.Core.Interfaces;

namespace Booking.Core.Entities;

public class UserRole : IHasId
{
    [Key]
    public Guid Id { get; set; }
    
    public required string Title { get; set; }
    
    public required bool CanEditOthersEvents { get; set; }
    
    public required bool IsAdmin { get; set; }
}