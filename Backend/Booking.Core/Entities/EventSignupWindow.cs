using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Booking.Core.Interfaces;

namespace Booking.Core.Entities;

public class EventSignupWindow : IHasId
{
    [Key]
    public Guid Id { get; set; }
    
    public Guid EventId { get; set; }
    [ForeignKey("EventId")] 
    public UserEvent Event { get; set; } = null!;
    
    public string Title { get; set; } = null!;
    
    public required DateOnly Date { get; set; }
    
    public required TimeOnly Time { get; set; }
    
    public required int MaxVisitors { get; set; }
}