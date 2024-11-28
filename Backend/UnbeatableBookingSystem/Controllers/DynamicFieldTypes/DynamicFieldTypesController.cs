using Booking.Application.Services;
using Booking.Core.DataQuery;
using Booking.Core.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UnbeatableBookingSystem.Controllers.Base;
using UnbeatableBookingSystem.Controllers.Base.Responses;
using UnbeatableBookingSystem.Controllers.DynamicFieldTypes.Requests;
using UnbeatableBookingSystem.Controllers.DynamicFieldTypes.Responses;

namespace UnbeatableBookingSystem.Controllers.DynamicFieldTypes;

[Route("/api/dynamic-field-types")]
public class DynamicFieldTypesController : Controller
{
    private readonly ControllerUtils _controllerUtils;
    private readonly BaseService<DynamicFieldType> _fieldTypeService;

    public DynamicFieldTypesController(ControllerUtils controllerUtils, BaseService<DynamicFieldType> fieldTypeService)
    {
        _controllerUtils = controllerUtils;
        _fieldTypeService = fieldTypeService;
    }
    
    [HttpGet]
    [ProducesResponseType(typeof(DynamicFieldTypesResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseStatusResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetFieldTypes()
    {
        var types = await _fieldTypeService.GetAsync(new DataQueryParams<DynamicFieldType>());
        var res = new DynamicFieldTypesResponse
        {
            Types = types.Select(t => new FieldTypeResponse
            {
                Id = t.Id,
                Title = t.Title
            }).ToArray()
        };
        return Ok(res);
    }
    
    [HttpPost]
    [Authorize]
    [ProducesResponseType(typeof(FieldTypeResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseStatusResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateFieldType([FromBody] CreateFieldTypeRequest request)
    {
        var check = await _controllerUtils.CheckUserIsAdminAsync(HttpContext);
        if (!check.Success)
        {
            return Forbid();
        }

        var fieldType = new DynamicFieldType
        {
            Id = Guid.NewGuid(),
            Title = request.Title
        };
        await _fieldTypeService.SaveAsync(fieldType);
        
        var res = new FieldTypeResponse
        {
            Id = fieldType.Id,
            Title = fieldType.Title
        };
        return Ok(res);
    }
    
    [HttpDelete("{typeId:guid}")]
    [ProducesResponseType(typeof(BaseStatusResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseStatusResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteField([FromRoute] Guid typeId)
    {
        var check = await _controllerUtils.CheckUserIsAdminAsync(HttpContext);
        if (!check.Success)
        {
            return Forbid();
        }
        var type = (await _fieldTypeService.GetAsync(new DataQueryParams<DynamicFieldType>
        {
            Expression = t => t.Id == typeId
        }))[0];
        
        await _fieldTypeService.TryRemoveAsync(type.Id);
        
        return Ok(new BaseStatusResponse
        {
            Completed = true,
            Status = "Успех",
            Message = "Тип динамического поля был удален."
        });
    }
}