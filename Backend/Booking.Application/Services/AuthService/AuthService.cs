using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Booking.Application.Utility;
using Booking.Core;
using Booking.Core.DataQuery;
using Booking.Core.Entities;
using Booking.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Booking.Application.Services.AuthService;

public class AuthService : IAuthService
{
    private readonly BaseService<User> _userService;
    private readonly IDbContextFactory<BookingDbContext> _dbContextFactory;
    private readonly BaseService<UserRole> _roleService;

    public AuthService(BaseService<User> userService, IDbContextFactory<BookingDbContext> dbContextFactory,
        BaseService<UserRole> roleService)
    {
        _userService = userService;
        _dbContextFactory = dbContextFactory;
        _roleService = roleService;
    }
    
    public async Task<(User? User, string RefreshToken, string AccessToken, string errorMsg)> TryLoginUserAsync(
        string email, string password)
    {
        var foundUsers = await _userService.GetAsync(new DataQueryParams<User>
        {
            Expression = u => u.Email == email,
            IncludeParams = new IncludeParams<User>
            {
                IncludeProperties = [u => u.Role]
            }
        });
        if (foundUsers.Length == 0 || 
            !PasswordHelper.VerifyPassword(foundUsers[0].PasswordHash, password))
        {
            return (null, string.Empty, string.Empty, 
                "No user with that email and password combination was found");
        }
        var user = foundUsers[0];
        var accesToken = GenerateAccessToken(user.Id, user.Role.Title);
        var refreshToken = new RefreshToken
        {
            UserId = user.Id,
            Token = GenerateRefreshToken(),
            ExpiryDate = DateTime.UtcNow.AddDays(7)
        };
        await SaveRefreshTokenAsync(refreshToken);
        return (user, refreshToken.Token, accesToken, string.Empty);
    }

    public async Task<(User User, string RefreshToken, string AccessToken)> RegisterUserOrThrowAsync(User user)
    {
        var foundUsers = await _userService.GetAsync(new DataQueryParams<User>
        {
            Expression = u => u.Email == user.Email || u.Phone == user.Phone
        });
        if (foundUsers.Length > 0)
        {
            throw new Exception("User with that email or phone is already registered.");
        }

        var userRole = (await _roleService.GetAsync(new DataQueryParams<UserRole>
        {
            Expression = r => r.Id == user.RoleId
        }))[0];
        var accessToken = GenerateAccessToken(user.Id, userRole.Title);
        var refreshToken = new RefreshToken
        {
            UserId = user.Id,
            Token = GenerateRefreshToken(),
            ExpiryDate = DateTime.UtcNow.AddDays(7)
        };
        await SaveRefreshTokenAsync(refreshToken);
        await _userService.SaveAsync(user);
        
        return (user, refreshToken.Token, accessToken);
    }

    public async Task<(User? User, string RefreshToken, string AccessToken, string errorMsg)> TryRefreshUsersTokens(string refreshToken)
    {
        var dbContext = await _dbContextFactory.CreateDbContextAsync();
        var existingRefreshToken = dbContext.RefreshTokens.FirstOrDefault(t => t.Token  == refreshToken);
        if (existingRefreshToken == null)
        {
            return (null, string.Empty, string.Empty, "Refresh token is invalid.");
        }

        if (existingRefreshToken.ExpiryDate < DateTime.UtcNow)
        {
            await RemoveRefreshTokenAsync(existingRefreshToken.UserId);
            return (null, string.Empty, string.Empty, "Refresh token is expired.");
        }
        
        var users = await _userService.GetAsync(new DataQueryParams<User>
        {
            Expression = u => u.Id == existingRefreshToken.UserId,
            IncludeParams = new IncludeParams<User>
            {
                IncludeProperties = [u => u.Role]
            }
        });
        if (users.Length < 1)
        {
            return (null, string.Empty, string.Empty, "The user does not exist.");
        }
        
        var user = users[0];
        var newRefreshToken = GenerateRefreshToken();
        existingRefreshToken.Token = newRefreshToken;
        existingRefreshToken.ExpiryDate = DateTime.UtcNow.AddDays(7);
        await SaveRefreshTokenAsync(existingRefreshToken);
        var newAccessToken = GenerateAccessToken(user.Id, user.Role.Title);
        return (user, newRefreshToken, newAccessToken, string.Empty);
    }

    public async Task RemoveRefreshTokenAsync(Guid userId)
    {
        var dbContext = await _dbContextFactory.CreateDbContextAsync();
        var refreshToken = dbContext.RefreshTokens.FirstOrDefault(t => t.UserId == userId);
        if (refreshToken != null)
        {
            dbContext.RefreshTokens.Remove(refreshToken);
            await dbContext.SaveChangesAsync();
        }
    }
    
    public async Task RevokeAccessToken(Guid jti, DateTime expirationTime)
    {
        var dbContext = await _dbContextFactory.CreateDbContextAsync();
        var existingEntry = dbContext.RevokedAccessTokens.FirstOrDefault(t => t.Jti == jti);
        if (existingEntry == null)
        {
            dbContext.RevokedAccessTokens.Add(new RevokedAccessToken
            {
                Jti = jti,
                ExpirationTime = expirationTime
            });
            await dbContext.SaveChangesAsync();
        }
    }

    private async Task SaveRefreshTokenAsync(RefreshToken token)
    {
        var dbContext = await _dbContextFactory.CreateDbContextAsync();
        var existingToken = dbContext.RefreshTokens.FirstOrDefault(t => t.UserId == token.UserId);
        if (existingToken != null)
        {
            existingToken.Token = token.Token;
            existingToken.ExpiryDate = DateTime.UtcNow.AddDays(7);
        }
        else
        {
            dbContext.RefreshTokens.Add(token);
        }
        await dbContext.SaveChangesAsync();
    }
    
    private string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }
    
    private string GenerateAccessToken(Guid userId, string roleTitle)
    {
        var claims = new[]
        {
            new Claim(AuthOptions.ClaimTypeUserId, userId.ToString()),
            new Claim(AuthOptions.ClaimTypeRole, roleTitle),
            new Claim(AuthOptions.ClaimTypeJti, Guid.NewGuid().ToString())
        };

        var key = AuthOptions.GetSymmetricSecurityKey();
        var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: AuthOptions.Issuer,
            audience: AuthOptions.Audience,
            claims: claims,
            expires: DateTime.Now.AddMinutes(15),
            signingCredentials: signingCredentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}