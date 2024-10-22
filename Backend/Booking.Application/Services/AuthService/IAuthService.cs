using Booking.Core.Entities;

namespace Booking.Application.Services.AuthService;

public interface IAuthService
{
    public Task<User?> TryLoginUserAsync(string email, string password);
    
    public Task<User> RegisterUserOrThrowAsync(User user);
}