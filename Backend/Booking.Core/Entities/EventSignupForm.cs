using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Booking.Core.Interfaces;

namespace Booking.Core.Entities;

public class EventSignupForm : IHasId
{
    [Key]
    public required Guid Id { get; set; }
    
    public required bool IsFioRequired { get; set; }
    
    public required bool IsPhoneRequired { get; set; }
    
    public required bool IsEmailRequired { get; set; }
    
    public required Guid EventId { get; set; }
    [ForeignKey("EventId")] 
    public UserEvent Event { get; set; } = null!;
}