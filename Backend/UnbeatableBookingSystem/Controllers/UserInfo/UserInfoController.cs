using Booking.Application.Services;
using Booking.Application.Services.AuthService;
using Booking.Application.Utility;
using Booking.Core;
using Booking.Core.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UnbeatableBookingSystem.Controllers.Base.Responses;
using UnbeatableBookingSystem.Controllers.UserInfo.Requests;
using UnbeatableBookingSystem.Controllers.UserInfo.Responses;
using UnbeatableBookingSystem.Utility;

namespace UnbeatableBookingSystem.Controllers.UserInfo;

[Route("/api/user/")]
[Authorize]
public class UserInfoController : Controller
{
    private readonly BaseService<User> _userService;
    private readonly BaseService<UserEvent> _eventsService;
    private readonly BaseService<UserRole> _roleService;
    private readonly IWebHostEnvironment _env;
    private readonly UserInfoService _userInfoService;
    private readonly IAuthService _authService;

    private readonly string _avatarImagesRelativePath = Path.Combine("images", "user-avatars");
    
    private readonly string _defaultAvatarFilename = "default-avatar.jpg";
    
    public UserInfoController(BaseService<User> userService, BaseService<UserEvent> eventsService,
        BaseService<UserRole> roleService, IWebHostEnvironment env, UserInfoService userInfoService,
        IAuthService authService)
    {
        _userService = userService;
        _eventsService = eventsService;
        _roleService = roleService;
        _env = env;
        _userInfoService = userInfoService;
        _authService = authService;
        var avatarImagesFullPath = Path.Combine(_env.WebRootPath, _avatarImagesRelativePath);
        if (!Directory.Exists(avatarImagesFullPath))
        {
            Directory.CreateDirectory(avatarImagesFullPath);
        }
    }
    
    /// <summary>
    /// Получить собственный профиль пользователя
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(SelfUserInfoResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseStatusResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetSelfUserProfile()
    {
        var info = await TryGetSelfUserAsync(true);
        if (!info.Succes || info.User == null)
        {
            return FailedRequest(info.ErrorMsg);
        }
        var user = info.User;
        var events = await _userInfoService.GetEventsForProfileAsync(user.Id, 10);
        
        var res = new SelfUserInfoResponse
        {
            Id = user.Id,
            Phone = user.Phone,
            Email = user.Email,
            Fio = user.Fio,
            RoleTitle = user.Role.Title,
            Description = user.Description,
            Status = user.UserStatus,
            AvatarImage = DtoConverter.GetAvatarUrl(user, _avatarImagesRelativePath, _defaultAvatarFilename, Request),
            CreatedEvents = events.Select(DtoConverter.ConvertEventToBasicInfo).ToArray()
        };
        return Ok(res);
    }
    
    /// <summary>
    /// Получить профиль пользователя по его id
    /// </summary>
    [HttpGet("{userId:guid}")]
    [ProducesResponseType(typeof(UserInfoResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseStatusResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetUserProfile([FromRoute] Guid userId)
    {
        var user = await _userService.GetByIdOrDefaultAsync(userId);
        if (user == null)
        {
            return NoUserFound();
        }
        var events = await _userInfoService.GetEventsForProfileAsync(user.Id, 10);

        var res = new UserInfoResponse
        {
            Id = user.Id,
            Fio = user.Fio,
            Description = user.Description,
            Status = user.UserStatus,
            AvatarImage = DtoConverter.GetAvatarUrl(user, _avatarImagesRelativePath, _defaultAvatarFilename, Request),
            CreatedEvents = events.Select(DtoConverter.ConvertEventToBasicInfo).ToArray()
        };
        return Ok(res);
    }
    
    /// <summary>
    /// Получить краткую информацию о пользователе по его id
    /// </summary>
    [HttpGet("{userId:guid}/brief")]
    [ProducesResponseType(typeof(BriefUserInfoResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseStatusResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetBriefUserInfo([FromRoute] Guid userId)
    {
        var user = await _userService.GetByIdOrDefaultAsync(userId);
        if (user == null)
        {
            return NoUserFound();
        }
        
        var res = new BriefUserInfoResponse()
        {
            Id = user.Id,
            Fio = user.Fio,
            AvatarImage = DtoConverter.GetAvatarUrl(user, _avatarImagesRelativePath, _defaultAvatarFilename, Request)
        };
        return Ok(res);
    }
    
    [HttpPut]
    [ProducesResponseType(typeof(BaseStatusResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseStatusResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateUserInfo([FromBody] UpdateUserInfoRequest dto)
    {
        var info = await TryGetSelfUserAsync();
        if (!info.Succes || info.User == null)
        {
            return FailedRequest(info.ErrorMsg);
        }
        var user = info.User;
        user.UserStatus = dto.Status;
        user.Description = dto.Description;
        user.Fio = dto.Fio;
        user.City = dto.City;
        await _userService.SaveAsync(user);

        return Ok(new BaseStatusResponse
        {
            Status = "Success",
            Message = "User info successfully updated.",
            Completed = true
        });
    }

    /// <summary>
    /// Обновить аватар пользователя
    /// </summary>
    [HttpPost("avatar")]
    [ProducesResponseType(typeof(UpdateAvatarResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseStatusResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateUserAvatar(IFormFile? file)
    {
        if (file == null)
        {
            return FailedRequest("Request doesn't contain file.");
        }
        var info = await TryGetSelfUserAsync();
        if (!info.Succes || info.User == null)
        {
            return FailedRequest(info.ErrorMsg);
        }
        var user = info.User;
        await _userInfoService.UpdateUserAvatarAsync(user, _env.WebRootPath, _avatarImagesRelativePath,
            _defaultAvatarFilename, file);

        return Ok(new UpdateAvatarResponse
        {
            Status = "Success",
            Message = "User avatar successfully updated.",
            Completed = true,
            Image = DtoConverter.GetAvatarUrl(user, _avatarImagesRelativePath, _defaultAvatarFilename, Request)
        });
    }
    
    /// <summary>
    /// Удалить аватар пользователя
    /// </summary>
    [HttpDelete("avatar")]
    [ProducesResponseType(typeof(UpdateAvatarResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseStatusResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> RemoveUserAvatar()
    {
        var info = await TryGetSelfUserAsync();
        if (!info.Succes || info.User == null)
        {
            return FailedRequest(info.ErrorMsg);
        }
        var user = info.User;
        await _userInfoService.UpdateUserAvatarAsync(user, _env.WebRootPath, _avatarImagesRelativePath,
            _defaultAvatarFilename, null);

        return Ok(new UpdateAvatarResponse
        {
            Status = "Success",
            Message = "User avatar successfully updated.",
            Completed = true,
            Image = DtoConverter.GetAvatarUrl(user, _avatarImagesRelativePath, _defaultAvatarFilename, Request),
        });
    }

    [HttpPut("password")]
    [ProducesResponseType(typeof(BaseStatusResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseStatusResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateUserPassword([FromBody] UpdatePasswordRequest dto)
    {
        var info = await TryGetSelfUserAsync();
        if (!info.Succes || info.User == null)
        {
            return FailedRequest(info.ErrorMsg);
        }
        var user = info.User;
        if (!PasswordHelper.VerifyPassword(user.PasswordHash, dto.CurrentPassword))
        {
            return FailedRequest("Current password is incorrect.");
        }

        user.PasswordHash = PasswordHelper.HashPassword(dto.NewPassword);
        await _userService.SaveAsync(user);

        return Ok(new BaseStatusResponse
        {
            Status = "Success",
            Message = "User's password successfully updated.",
            Completed = true
        });
    }
    
    [HttpPut("phone")]
    [ProducesResponseType(typeof(BaseStatusResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseStatusResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateUserPhone([FromBody] UpdatePhoneRequest dto)
    {
        var info = await TryGetSelfUserAsync();
        if (!info.Succes || info.User == null)
        {
            return FailedRequest(info.ErrorMsg);
        }
        var user = info.User;
        
        user.Phone = dto.Phone;
        await _userService.SaveAsync(user);

        return Ok(new BaseStatusResponse
        {
            Status = "Success",
            Message = "User's phone successfully updated.",
            Completed = true
        });
    }
    
    [HttpPut("email")]
    [ProducesResponseType(typeof(BaseStatusResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseStatusResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateUserEmail([FromBody] UpdateEmailRequest dto)
    {
        var info = await TryGetSelfUserAsync();
        if (!info.Succes || info.User == null)
        {
            return FailedRequest(info.ErrorMsg);
        }
        var user = info.User;
        
        // TODO: Подтверждение почты...
        // На почту отправляется ссылка с встроенным guid, которая подтверждает смену почты, а до этого создаётся заявка смены почты и висит pending
        
        user.Email = dto.Email;
        await _userService.SaveAsync(user);

        return Ok(new BaseStatusResponse
        {
            Status = "Success",
            Message = "User's email successfully updated.",
            Completed = true
        });
    }
    
    private BadRequestObjectResult NoUserFound()
    {
        return BadRequest(new BaseStatusResponse
        {
            Status = "Failed",
            Message = "User with provided userId doesn't exist.",
            Completed = false
        });
    }
    
    private BadRequestObjectResult FailedRequest(string msg)
    {
        return BadRequest(new BaseStatusResponse
        {
            Status = "Failed",
            Message = msg,
            Completed = false
        });
    }
    
    private async Task<(bool Succes, User? User, string ErrorMsg)> TryGetSelfUserAsync(bool includeRole = false)
    {
        var idClaim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == AuthOptions.ClaimTypeUserId);
        if (idClaim == null)
        {
            return (false, null, "User doesn't have proper claims, unauthorized.");
        }
        var user = await _userService.GetByIdOrDefaultAsync(new Guid(idClaim.Value));
        if (user == null)
        {
            await _authService.RemoveRefreshTokenAsync(new Guid(idClaim.Value));
            return (false, null, "Operation failed. No user with that id was found. Unauthorized.");
        }

        if (includeRole)
        {
            var role = await _roleService.GetByIdOrDefaultAsync(user.RoleId);
            user.Role = role!;
        }
        
        return (true, user, "");
    }
}