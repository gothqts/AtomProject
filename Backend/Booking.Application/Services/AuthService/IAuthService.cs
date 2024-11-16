using Booking.Core.Entities;

namespace Booking.Application.Services.AuthService;

public interface IAuthService
{
    public Task<(User? User, string RefreshToken, string AccessToken, string errorMsg)> TryLoginUserAsync(string email, string password);
    
    public Task<(User User, string RefreshToken, string AccessToken)> RegisterUserOrThrowAsync(User user);

    public Task<(User? User, string RefreshToken, string AccessToken, string errorMsg)> TryRefreshUsersTokens(string refreshToken);
    
    public Task RemoveRefreshTokenAsync(Guid userId);

    public Task RevokeAccessToken(Guid jti, DateTime expirationTime);
}