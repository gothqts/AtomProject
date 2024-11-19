using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Booking.Core.Interfaces;

namespace Booking.Core.Entities;

public class EventSignupEntry : IHasId
{
    [Key]
    public Guid Id { get; set; }
    
    public required Guid SignupWindowId { get; set; }
    [ForeignKey("SignupWindowId")]
    public EventSignupWindow SignupWindow { get; set; } = null!;

    public required Guid UserId { get; set; }
    [ForeignKey("UserId")]
    public User User { get; set; } = null!;
    
    public string? Phone { get; set; } = null!;
    
    public string? Fio { get; set; } = null!;
    
    public string? Email { get; set; } = null!;
}