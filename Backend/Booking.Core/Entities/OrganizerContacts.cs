using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Booking.Core.Interfaces;

namespace Booking.Core.Entities;

public class OrganizerContacts : IHasId
{
    [Key]
    public required Guid Id { get; set; }
    
    public required Guid EventId { get; set; }
    [ForeignKey("EventId")] 
    public UserEvent Event { get; set; } = null!;
    
    public required string Email { get; set; }
    
    public string Fio { get; set; } = null!;

    public string Phone { get; set; } = null!;
    
    public string Telegram { get; set; } = null!;
}