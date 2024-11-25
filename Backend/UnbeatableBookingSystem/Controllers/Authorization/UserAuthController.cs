using Booking.Application.Services;
using Booking.Application.Services.AuthService;
using Booking.Application.Utility;
using Booking.Core;
using Booking.Core.DataQuery;
using Booking.Core.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UnbeatableBookingSystem.Controllers.Authorization.Responses;
using UnbeatableBookingSystem.Controllers.Base.Responses;

namespace UnbeatableBookingSystem.Controllers.Authorization;

[Route("/api/auth/")]
public class UserAuthController : Controller
{
    private readonly IAuthService _authService;
    private readonly BaseService<UserRole> _roleService;
    private readonly BaseService<User> _userService;

    public UserAuthController(IAuthService authService, BaseService<UserRole> roleService,
        BaseService<User> userService)
    {
        _authService = authService;
        _roleService = roleService;
        _userService = userService;
    }
    
    /// <summary>
    /// Авторизация пользователя по email и паролю.
    /// </summary>
    [HttpPost("login")]
    [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseStatusResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> LoginUserWithEmail([FromBody] Requests.LoginRequest dto)
    {
        if (User.Identity!.IsAuthenticated)
        {
            return BadRequest(new BaseStatusResponse
            {
                Completed = false,
                Status = "Failed",
                Message = "User is already authenticated."
            });
        }
        
        var loginInfo = await _authService.TryLoginUserAsync(dto.Email, dto.Password);
        if (loginInfo.User == null)
        {
            return BadRequest(new LoginResponse
            {
                UserId = null,
                Status = "Failed",
                Message = loginInfo.errorMsg,
                Completed = false,
                AccessToken = string.Empty
            });
        }
        WriteRefreshTokenToCookies(loginInfo.RefreshToken);
        
        var res = new LoginResponse
        {
            UserId = loginInfo.User.Id,
            Status = "Success",
            Message = "User successfully authorized.",
            Completed = true,
            AccessToken = loginInfo.AccessToken
        };
        return Ok(res);
    }
    
    /// <summary>
    /// Обновление access и refresh токенов пользователя
    /// </summary>
    [HttpPost("refresh")]
    [ProducesResponseType(typeof(RefreshResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseStatusResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Refresh()
    {
        if (!Request.Cookies.TryGetValue(AuthOptions.RefreshTokenCookieName, out var refreshToken))
        {
            return Unauthorized(new BaseStatusResponse
            {
                Completed = false,
                Status = "Failed",
                Message = "Refresh token not found in cookies."
            });
        }
        await RevokeAccessTokenAsync();
        var refreshInfo = await _authService.TryRefreshUsersTokens(refreshToken);
        if (refreshInfo.User == null)
        {
            return Unauthorized(new BaseStatusResponse
            {
                Completed = false,
                Status = "Failed",
                Message = refreshInfo.errorMsg
            });
        }
        WriteRefreshTokenToCookies(refreshInfo.RefreshToken);
        return Ok(new RefreshResponse
        {
            AccessToken = refreshInfo.AccessToken
        });
    }
    
    /// <summary>
    /// Выход из аккаунта пользователя.
    /// </summary>
    [HttpPost("logout")]
    [Authorize]
    [ProducesResponseType(typeof(BaseStatusResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> LogoutUser()
    {
        var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == AuthOptions.ClaimTypeUserId);
        
        if (userIdClaim == null)
        {
            return Unauthorized(new BaseStatusResponse
            {
                Status = "Failed",
                Message = "Access token is invalid.",
                Completed = false
            });
        }
        var userId = new Guid(userIdClaim.Value);
        await RevokeAccessTokenAsync();
        await _authService.RemoveRefreshTokenAsync(userId);
        
        var res = new BaseStatusResponse
        {
            Status = "Success",
            Message = "User successfully logged out.",
            Completed = true
        };
        return Ok(res);
    }
    
    /// <summary>
    /// Удалить аккаунт пользователя.
    /// </summary>
    [HttpPost("delete")]
    [Authorize]
    [ProducesResponseType(typeof(BaseStatusResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> DeleteUser()
    {
        var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == AuthOptions.ClaimTypeUserId);
        RemoveRefreshTokenFromCookies();
        
        if (userIdClaim == null)
        {
            return BadRequest(new BaseStatusResponse
            {
                Status = "Failed",
                Message = "Access token is invalid.",
                Completed = false
            });
        }
        var userId = new Guid(userIdClaim.Value);
        
        await RevokeAccessTokenAsync();
        await _authService.RemoveRefreshTokenAsync(userId);
        await _userService.TryRemoveAsync(userId);
        
        var res = new BaseStatusResponse
        {
            Status = "Success",
            Message = "User successfully deleted.",
            Completed = true
        };
        return Ok(res);
    }
    
    /// <summary>
    /// Регистрация Пользователя.
    /// </summary>
    [HttpPost("register")]
    [ProducesResponseType(typeof(RegisterResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseStatusResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> RegisterUser([FromBody] Requests.RegisterRequest dto)
    {
        if (User.Identity!.IsAuthenticated)
        {
            return BadRequest(new BaseStatusResponse
            {
                Completed = false,
                Status = "Failed",
                Message = "User is already authenticated."
            });
        }
        
        var role = await _roleService.GetAsync(new DataQueryParams<UserRole>
        {
            Expression = r => r.Title == "User"
        });
        
        var user = new User
        {
            Id = Guid.NewGuid(),
            Phone = "",
            Fio = dto.Fio,
            Email = dto.Email,
            PasswordHash = PasswordHelper.HashPassword(dto.Password),
            RoleId = role[0].Id,
            UserStatus = dto.Status ?? "",
            Description = "",
            City = dto.City ?? "",
            AvatarImageFilepath = ""
        };
        var foundUsers = await _userService.GetAsync(new DataQueryParams<User>
        {
            Expression = u => u.Email == user.Email
        });
        if (foundUsers.Length > 0)
        {
            return BadRequest(new RegisterResponse
            {
                Status = "Failed",
                Message = "User with that email is already registered.",
                UserId = null,
                Completed = false,
                AccessToken = string.Empty
            });
        }
        try
        {
            var registerInfo = await _authService.RegisterUserOrThrowAsync(user);
            
            var res = new RegisterResponse
            {
                Status = "Success",
                Message = "User successfully registered.",
                UserId = registerInfo.User.Id,
                Completed = true,
                AccessToken = registerInfo.AccessToken
            };
            WriteRefreshTokenToCookies(registerInfo.RefreshToken);
            return Ok(res);
        }
        catch (Exception e)
        {
            return StatusCode(500, new RegisterResponse
            {
                Status = "Failed",
                Message = $"Registration failed. Info: {e.Message}",
                UserId = null,
                Completed = false,
                AccessToken = string.Empty
            });
        }
    }
    
    private async Task RevokeAccessTokenAsync()
    {
        var jtiClaim = User.Claims.First(c => c.Type == AuthOptions.ClaimTypeJti);
        var expTimeClaim = User.Claims.First(c => c.Type == AuthOptions.ClaimTypeExpireTime);
        var jti = Guid.Parse(jtiClaim.Value);
        var expTime = DateTimeOffset.FromUnixTimeSeconds(long.Parse(expTimeClaim.Value)).UtcDateTime;
        RemoveRefreshTokenFromCookies();
        await _authService.RevokeAccessToken(jti, expTime);
    }
    
    private void WriteRefreshTokenToCookies(string token)
    {
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            Expires = DateTime.UtcNow.AddDays(7)
        };
        Response.Cookies.Append(AuthOptions.RefreshTokenCookieName, token, cookieOptions);
    }
    
    private void RemoveRefreshTokenFromCookies()
    {
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            Expires = DateTime.UtcNow.AddDays(-1)
        };
        Response.Cookies.Append(AuthOptions.RefreshTokenCookieName, "", cookieOptions);
    }
}