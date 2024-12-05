using Booking.Application.Services;
using Booking.Core.DataQuery;
using Booking.Core.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UnbeatableBookingSystem.Controllers.Admin.Requests;
using UnbeatableBookingSystem.Controllers.Admin.Responses;
using UnbeatableBookingSystem.Controllers.Base;
using UnbeatableBookingSystem.Controllers.Base.Responses;

namespace UnbeatableBookingSystem.Controllers.Admin;

[Route("/api/admin")]
public class AdminController : Controller
{
    private readonly ControllerUtils _controllerUtils;
    private readonly BaseService<UserRole> _roleService;
    private readonly BaseService<User> _userService;

    public AdminController(ControllerUtils controllerUtils, BaseService<UserRole> roleService, BaseService<User> userService)
    {
        _controllerUtils = controllerUtils;
        _roleService = roleService;
        _userService = userService;
    }
    
    [HttpGet("roles")]
    [Authorize]
    [ProducesResponseType(typeof(RolesListResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseStatusResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetRoles()
    {
        var check = await _controllerUtils.CheckUserIsAdminAsync(HttpContext);
        if (!check.Success)
        {
            return Forbid();
        }
        var roles = await _roleService.GetAsync(new DataQueryParams<UserRole>());
        var res = new RolesListResponse
        {
            Roles = roles.Select(r => new RoleResponse
            {
                Id = r.Id,
                Title = r.Title,
                CanEditOthersEvents = r.CanEditOthersEvents,
                IsAdmin = r.IsAdmin
            }).ToArray()
        };
        return Ok(res);
    }
    
    [HttpPost("change-role/{userId:guid}")]
    [Authorize]
    [ProducesResponseType(typeof(BaseStatusResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseStatusResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ChangeUserRole([FromRoute] Guid userId, [FromBody] ChangeRoleRequest request)
    {
        var check = await _controllerUtils.CheckUserIsAdminAsync(HttpContext);
        if (!check.Success)
        {
            return Forbid();
        }

        var roles = await _roleService.GetAsync(new DataQueryParams<UserRole>
        {
            Expression = r => r.Id == request.RoleId
        });
        if (roles.Length != 1)
        {
            return CustomResults.FailedRequest("Не было найдено роли с указанным id");
        }
        var role = roles[0];
        
        var user = await _userService.GetByIdOrDefaultAsync(userId);
        if (user == null)
        {
            return CustomResults.NoUserFound();
        }
        if (check.User!.Id == user.Id)
        {
            return CustomResults.FailedRequest("Вы не можете менять свою роль.");
        }

        user.RoleId = role.Id;
        await _userService.SaveAsync(user);

        var res = new BaseStatusResponse
        {
            Completed = true,
            Status = "Успех",
            Message = $"Пользователю {user.Fio} присвоена роль \"{role.Title}\"."
        };
        return Ok(res);
    }
}