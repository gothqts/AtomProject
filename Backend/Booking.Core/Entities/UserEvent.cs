using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Booking.Core.Interfaces;

namespace Booking.Core.Entities;

public class UserEvent : IHasId
{
    [Key]
    public Guid Id { get; set; }
    
    public required Guid CreatorUserId { get; set; }
    [ForeignKey("CreatorUserId")] 
    public User CreatorUser { get; set; } = null!;
    
    public required bool IsPublic { get; set; }
    
    public required string Title { get; set; }

    public string PathToBannerImage { get; set; } = null!;
    
    public required bool IsOnline { get; set; }
    
    public required bool IsSignupOpened { get; set; }

    public string City { get; set; } = null!;

    public string Address { get; set; } = null!;
    
    public required DateTime DateStart { get; set; }
    
    public required DateTime DateEnd { get; set; }

    public string Description { get; set; } = null!;
}