using System.Security.Claims;
using Booking.Application.Services;
using Booking.Core.DataQuery;
using Booking.Core.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
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

    public UserInfoController(BaseService<User> userService, BaseService<UserEvent> eventsService,
        BaseService<UserRole> roleService)
    {
        _userService = userService;
        _eventsService = eventsService;
        _roleService = roleService;
    }
    
    /// <summary>
    /// Получить собственный профиль пользователя
    /// </summary>
    [HttpGet("info")]
    [ProducesResponseType(typeof(UserInfoResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseStatusResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetSelfUserProfile()
    {
        var idClaim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
        if (idClaim == null)
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return BadRequest(new BaseStatusResponse
            {
                Status = "Failed",
                Message = "User doesn't have proper claims, unauthorized."
            });
        }
        
        var user = await _userService.GetByIdOrDefaultAsync(new Guid(idClaim.Value));
        if (user == null)
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return BadRequest(new BaseStatusResponse
            {
                Status = "Failed",
                Message = "Operation failed. No user with that id was found."
            });
        }

        var role = await _roleService.GetByIdOrDefaultAsync(user.RoleId);
        var events = await _eventsService.GetAsync(new DataQueryParams<UserEvent>
        {
            Expression = e => e.CreatorUserId == user.Id,
            Paging = new PagingParams
            {
                Skip = 0,
                Take = 10
            },
            Sorting = new SortingParams<UserEvent>
            {
                OrderBy = e => e.DateStart,
                PropertyName = "DateStart",
                Ascending = true
            }
        });
        
        var res = new UserInfoResponse
        {
            Id = user.Id,
            Phone = user.Phone,
            Email = user.Email,
            Fio = user.Fio,
            RoleTitle = role?.Title,
            Description = user.Description,
            Status = user.UserStatus,
            AvatarImage = user.PathToAvatarImage,
            CreatedEvents = events.Select(DtoConverter.ConvertEventToBasicInfo).ToArray()
        };
        return Ok(res);
    }

    [HttpPut("update")]
    [ProducesResponseType(typeof(BaseStatusResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseStatusResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateUserInfo([FromBody] UpdateUserInfoRequest dto)
    {
        var idClaim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
        if (idClaim == null)
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return BadRequest(new BaseStatusResponse
            {
                Status = "Failed",
                Message = "User doesn't have proper claims, unauthorized."
            });
        }
        
        var user = await _userService.GetByIdOrDefaultAsync(new Guid(idClaim.Value));
        if (user == null)
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return BadRequest(new BaseStatusResponse
            {
                Status = "Failed",
                Message = "Operation failed. No user with that id was found."
            });
        }

        user.UserStatus = dto.Status;
        user.Description = dto.Description;
        user.PathToAvatarImage = dto.AvatarImage;
        user.Fio = dto.Fio;
        await _userService.SaveAsync(user);

        return Ok(new BaseStatusResponse
        {
            Status = "Success",
            Message = "User info successfully updated."
        });
    }
}