using System.Linq.Expressions;
using Booking.Application.Utility;
using Booking.Core.DataQuery;
using Booking.Core.Entities;
using Microsoft.AspNetCore.Http;

namespace Booking.Application.Services.AuthService;

public class AuthService : IAuthService
{
    private readonly BaseService<User> _userService;

    public AuthService(BaseService<User> userService)
    {
        _userService = userService;
    }
    
    public async Task<User?> TryLoginUserAsync(string email, string password)
    {
        var foundUsers = await _userService.GetAsync(new DataQueryParams<User>
        {
            Expression = u => u.Email == email
        });
        if (foundUsers.Length != 1)
        {
            return null;
        }

        var foundUser = foundUsers[0];
        return PasswordHelper.VerifyPassword(foundUser.PasswordHash, password) ? foundUser : null;
    }

    public async Task<User> RegisterUserOrThrowAsync(User user)
    {
        var foundUsers = await _userService.GetAsync(new DataQueryParams<User>
        {
            Expression = u => u.Email == user.Email || u.Phone == user.Phone
        });
        if (foundUsers.Length > 0)
        {
            throw new Exception("User with that email or phone is already registered.");
        }

        await _userService.SaveAsync(user);
        
        return user;
    }
}