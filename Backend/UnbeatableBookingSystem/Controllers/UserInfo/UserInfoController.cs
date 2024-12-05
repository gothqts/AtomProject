using Booking.Application.Services;
using Booking.Application.Utility;
using Booking.Core.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UnbeatableBookingSystem.Controllers.Base;
using UnbeatableBookingSystem.Controllers.Base.Responses;
using UnbeatableBookingSystem.Controllers.UserInfo.Requests;
using UnbeatableBookingSystem.Controllers.UserInfo.Responses;
using UnbeatableBookingSystem.Utility;

namespace UnbeatableBookingSystem.Controllers.UserInfo;

[Route("/api/user/")]
public class UserInfoController : Controller
{
    private readonly BaseService<User> _userService;
    private readonly IWebHostEnvironment _env;
    private readonly UserInfoService _userInfoService;
    private readonly ControllerUtils _controllerUtils;

    public UserInfoController(BaseService<User> userService, IWebHostEnvironment env, 
        UserInfoService userInfoService, ControllerUtils controllerUtils)
    {
        _userService = userService;
        _env = env;
        _userInfoService = userInfoService;
        _controllerUtils = controllerUtils;
        var avatarImagesFullPath = Path.Combine(_env.WebRootPath, _userInfoService.AvatarImagesRelativePath);
        if (!Directory.Exists(avatarImagesFullPath))
        {
            Directory.CreateDirectory(avatarImagesFullPath);
        }
    }
    
    /// <summary>
    /// Получить собственный профиль пользователя
    /// </summary>
    [HttpGet]
    [Authorize]
    [ProducesResponseType(typeof(SelfUserInfoResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseStatusResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetSelfUserProfile()
    {
        var info = await _controllerUtils.TryGetSelfUserAsync(HttpContext, true);
        if (!info.Success || info.User == null)
        {
            return CustomResults.FailedRequest(info.ErrorMsg);
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
            AvatarImage = DtoConverter.GetAvatarUrl(user, _userInfoService.AvatarImagesRelativePath, _userInfoService.DefaultAvatarFilename, Request),
            CreatedEvents = events.Select(e => DtoConverter.ConvertEventToBasicInfo(e, 
                    _userInfoService.AvatarImagesRelativePath, _userInfoService.DefaultAvatarFilename, Request))
                .ToArray()
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
            return CustomResults.NoUserFound();
        }
        var events = await _userInfoService.GetEventsForProfileAsync(user.Id, 10);

        var res = new UserInfoResponse
        {
            Id = user.Id,
            Fio = user.Fio,
            Description = user.Description,
            Status = user.UserStatus,
            AvatarImage = DtoConverter.GetAvatarUrl(user, _userInfoService.AvatarImagesRelativePath, 
                _userInfoService.DefaultAvatarFilename, Request),
            CreatedEvents = events.Select(e => DtoConverter.ConvertEventToBasicInfo(e, 
                    _userInfoService.AvatarImagesRelativePath, _userInfoService.DefaultAvatarFilename, Request))
                .ToArray()
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
            return CustomResults.NoUserFound();
        }
        
        var res = new BriefUserInfoResponse()
        {
            Id = user.Id,
            Fio = user.Fio,
            AvatarImage = DtoConverter.GetAvatarUrl(user, _userInfoService.AvatarImagesRelativePath, 
                _userInfoService.DefaultAvatarFilename, Request)
        };
        return Ok(res);
    }
    
    [HttpPut]
    [Authorize]
    [ProducesResponseType(typeof(BaseStatusResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseStatusResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateUserInfo([FromBody] UpdateUserInfoRequest dto)
    {
        var info = await _controllerUtils.TryGetSelfUserAsync(HttpContext);
        if (!info.Success || info.User == null)
        {
            return CustomResults.FailedRequest(info.ErrorMsg);
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
    [Authorize]
    [ProducesResponseType(typeof(UpdateAvatarResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseStatusResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateUserAvatar(IFormFile? file)
    {
        if (file == null)
        {
            return CustomResults.FailedRequest("Request doesn't contain file.");
        }
        var info = await _controllerUtils.TryGetSelfUserAsync(HttpContext);
        if (!info.Success || info.User == null)
        {
            return CustomResults.FailedRequest(info.ErrorMsg);
        }
        var user = info.User;
        await _userInfoService.UpdateUserAvatarAsync(user, _env.WebRootPath, _userInfoService.AvatarImagesRelativePath,
            _userInfoService.DefaultAvatarFilename, file);

        return Ok(new UpdateAvatarResponse
        {
            Status = "Success",
            Message = "User avatar successfully updated.",
            Completed = true,
            Image = DtoConverter.GetAvatarUrl(user, _userInfoService.AvatarImagesRelativePath, 
                _userInfoService.DefaultAvatarFilename, Request)
        });
    }
    
    /// <summary>
    /// Удалить аватар пользователя
    /// </summary>
    [HttpDelete("avatar")]
    [Authorize]
    [ProducesResponseType(typeof(UpdateAvatarResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseStatusResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> RemoveUserAvatar()
    {
        var info = await _controllerUtils.TryGetSelfUserAsync(HttpContext);
        if (!info.Success || info.User == null)
        {
            return CustomResults.FailedRequest(info.ErrorMsg);
        }
        var user = info.User;
        await _userInfoService.UpdateUserAvatarAsync(user, _env.WebRootPath, _userInfoService.AvatarImagesRelativePath,
            _userInfoService.DefaultAvatarFilename, null);

        return Ok(new UpdateAvatarResponse
        {
            Status = "Success",
            Message = "User avatar successfully updated.",
            Completed = true,
            Image = DtoConverter.GetAvatarUrl(user, _userInfoService.AvatarImagesRelativePath, 
                _userInfoService.DefaultAvatarFilename, Request),
        });
    }

    [HttpPut("password")]
    [Authorize]
    [ProducesResponseType(typeof(BaseStatusResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseStatusResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateUserPassword([FromBody] UpdatePasswordRequest dto)
    {
        var info = await _controllerUtils.TryGetSelfUserAsync(HttpContext);
        if (!info.Success || info.User == null)
        {
            return CustomResults.FailedRequest(info.ErrorMsg);
        }
        var user = info.User;
        if (!PasswordHelper.VerifyPassword(user.PasswordHash, dto.CurrentPassword))
        {
            return CustomResults.FailedRequest("Current password is incorrect.");
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
    [Authorize]
    [ProducesResponseType(typeof(BaseStatusResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseStatusResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateUserPhone([FromBody] UpdatePhoneRequest dto)
    {
        var info = await _controllerUtils.TryGetSelfUserAsync(HttpContext);
        if (!info.Success || info.User == null)
        {
            return CustomResults.FailedRequest(info.ErrorMsg);
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
    [Authorize]
    [ProducesResponseType(typeof(BaseStatusResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseStatusResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateUserEmail([FromBody] UpdateEmailRequest dto)
    {
        var info = await _controllerUtils.TryGetSelfUserAsync(HttpContext);
        if (!info.Success || info.User == null)
        {
            return CustomResults.FailedRequest(info.ErrorMsg);
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
    
    
}