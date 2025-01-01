using UnbeatableBookingSystem.Controllers.Events.Responses;

namespace UnbeatableBookingSystem.Controllers.MyEvents.Responses;

public class EventResponse
{
    public Guid Id { get; set; }
    
    public DateTime CreationDate { get; set; }
    
    public required bool IsPublic { get; set; }
    
    public required string Title { get; set; }

    public required string BannerImage { get; set; }
    
    public required bool IsOnline { get; set; }
    
    public required bool IsSignupOpened { get; set; }

    public string? City { get; set; } = null!;

    public string? Address { get; set; } = null!;
    
    public required DateTime DateStart { get; set; }
    
    public required DateTime DateEnd { get; set; }

    public string Description { get; set; } = null!;

    public required SignupWindowResponse[] SignupWindows { get; set; }
    
    public required EventFormResponse SignupForm { get; set; }
    
    public required ContactsResponse[] Contacts { get; set; }
}