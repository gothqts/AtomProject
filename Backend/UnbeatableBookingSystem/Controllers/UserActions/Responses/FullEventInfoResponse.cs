namespace UnbeatableBookingSystem.Controllers.UserActions.Responses;

public class FullEventInfoResponse
{
    public Guid Id { get; set; }
    
    public required bool IsPublic { get; set; }
    
    public required string Title { get; set; }

    public string BannerImageFilepath { get; set; } = null!;
    
    public required bool IsOnline { get; set; }
    
    public required bool IsSignupOpened { get; set; }

    public string? City { get; set; } = null!;

    public string? Address { get; set; } = null!;
    
    public required DateTime DateStart { get; set; }
    
    public required DateTime DateEnd { get; set; }

    public string Description { get; set; } = null!;

    public ContactsResponse[] OrganizerContacts { get; set; } = null!;
    
    public required SignupWindowResponse[] SignupWindows { get; set; }
}