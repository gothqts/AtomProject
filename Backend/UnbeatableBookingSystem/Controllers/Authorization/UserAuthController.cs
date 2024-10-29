using System.Security.Claims;
using Booking.Application.Services;
using Booking.Application.Services.AuthService;
using Booking.Application.Utility;
using Booking.Core.DataQuery;
using Booking.Core.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using UnbeatableBookingSystem.Controllers.Authorization.Responses;
using UnbeatableBookingSystem.Controllers.Base.Responses;

namespace UnbeatableBookingSystem.Controllers.Authorization;

[Route("/api/auth/")]
public class UserAuthController : Controller
{
    private readonly IAuthService _authService;
    private readonly BaseService<UserRole> _roleService;

    public UserAuthController(IAuthService authService, BaseService<UserRole> roleService)
    {
        _authService = authService;
        _roleService = roleService;
    }
    
    /// <summary>
    /// Авторизация пользователя по email и паролю.
    /// </summary>
    [HttpPost("login")]
    [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseStatusResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> LoginUserWithEmail([FromBody] Requests.LoginRequest dto)
    {
        if (HttpContext.User.Identity.IsAuthenticated)
        {
            return BadRequest(new LoginResponse
            {
                UserId = null,
                Status = "Failed",
                Message = "User is already authorized.",
                Completed = false
            });
        }
        
        var user = await _authService.TryLoginUserAsync(dto.Email, dto.Password);
        if (user == null)
        {
            return BadRequest(new LoginResponse
            {
                UserId = null,
                Status = "Failed",
                Message = "Authorization failed. No user with that email and password combination was found.",
                Completed = false
            });
        }
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
        };
        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);

        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

        var res = new LoginResponse
        {
            UserId = user.Id,
            Status = "Success",
            Message = "User successfully authorized.",
            Completed = true
        };
        return Ok(res);
    }
    
    /// <summary>
    /// Выход из аккаунта пользователя.
    /// </summary>
    [HttpPost("logout")]
    [ProducesResponseType(typeof(BaseStatusResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> LogoutUser()
    {
        if (!HttpContext.User.Identity.IsAuthenticated)
        {
            return BadRequest(new BaseStatusResponse
            {
                Status = "Failed",
                Message = "User is already logged out.",
                Completed = false
            });
        }
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

        var res = new BaseStatusResponse
        {
            Status = "Success",
            Message = "User successfully logged out.",
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
        var role = await _roleService.GetAsync(new DataQueryParams<UserRole>
        {
            Expression = r => r.Title == "User"
        });
        
        var user = new User
        {
            Id = Guid.NewGuid(),
            Phone = "",
            Fio = dto.FIO,
            Email = dto.Email,
            PasswordHash = PasswordHelper.HashPassword(dto.Password),
            RoleId = role[0].Id,
            UserStatus = dto.Status ?? "",
            Description = "",
            City = dto.City ?? "",
            AvatarImageFilepath = ""
        };
        try
        {
            var registeredUser = await _authService.RegisterUserOrThrowAsync(user);
            
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, registeredUser.Email),
                new Claim(ClaimTypes.NameIdentifier, registeredUser.Id.ToString())
            };
            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            var res = new RegisterResponse
            {
                Status = "Success",
                Message = "User successfully registered.",
                UserId = registeredUser.Id,
                Completed = true
            };
            return Ok(res);
        }
        catch (Exception e)
        {
            return BadRequest(new RegisterResponse
            {
                Status = "Failed",
                Message = $"Registration failed. Info: {e.Message}",
                UserId = null,
                Completed = false
            });
        }
    }
}