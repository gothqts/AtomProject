using Booking.Application.Services;
using Booking.Core.DataQuery;
using Booking.Core.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UnbeatableBookingSystem.Controllers.Base;
using UnbeatableBookingSystem.Controllers.Base.Responses;
using UnbeatableBookingSystem.Controllers.DynamicFields.Requests;
using UnbeatableBookingSystem.Controllers.DynamicFields.Responses;
using UnbeatableBookingSystem.Controllers.UserActions.Responses;
using UnbeatableBookingSystem.Utility;

namespace UnbeatableBookingSystem.Controllers.DynamicFields;

[Route("/api/my-events/{eventId:guid}/form/fields")]
[Authorize]
public class DynamicFieldsController : Controller
{
    private readonly ControllerUtils _controllerUtils;
    private readonly BaseService<FormDynamicField> _fieldsService;
    private readonly BaseService<EventSignupForm> _formService;
    private readonly BaseService<DynamicFieldType> _fieldTypeService;
    private readonly BaseService<EntryFieldValue> _fieldValuesService;

    public DynamicFieldsController(ControllerUtils controllerUtils, BaseService<FormDynamicField> fieldsService,
        BaseService<EventSignupForm> formService, BaseService<DynamicFieldType> fieldTypeService,
        BaseService<EntryFieldValue> fieldValuesService)
    {
        _controllerUtils = controllerUtils;
        _fieldsService = fieldsService;
        _formService = formService;
        _fieldTypeService = fieldTypeService;
        _fieldValuesService = fieldValuesService;
    }
    
    [HttpGet]
    [ProducesResponseType(typeof(DynamicFieldsResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseStatusResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetFields([FromRoute] Guid eventId)
    {
        var check = await _controllerUtils.CheckUserCanEditEventAsync(HttpContext, eventId);
        if (!check.Success)
        {
            return CustomResults.FailedRequest(check.ErrorMsg);
        }

        var form = (await _formService.GetAsync(new DataQueryParams<EventSignupForm>
        {
            Expression = f => f.EventId == eventId
        }))[0];
        var fields = await _fieldsService.GetAsync(new DataQueryParams<FormDynamicField>
        {
            Expression = f => f.EventFormId == form.Id,
            IncludeParams = new IncludeParams<FormDynamicField>
            {
                IncludeProperties = [f => f.FieldType]
            }
        });
        
        var res = new DynamicFieldsResponse
        {
            Fields = fields.Select(DtoConverter.DynamicFieldToResponse).ToArray()
        };
        return Ok(res);
    }
    
    [HttpPost]
    [ProducesResponseType(typeof(FormFieldResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseStatusResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateField([FromRoute] Guid eventId)
    {
        var check = await _controllerUtils.CheckUserCanEditEventAsync(HttpContext, eventId);
        if (!check.Success)
        {
            return CustomResults.FailedRequest(check.ErrorMsg);
        }

        var fieldTypes = await _fieldTypeService.GetAsync(new DataQueryParams<DynamicFieldType>());
        var field = new FormDynamicField
        {
            Id = Guid.NewGuid(),
            Title = "Новое поле",
            IsRequired = false,
            FieldTypeId = fieldTypes[0].Id,
            MaxSymbols = null,
            MinValue = null,
            MaxValue = null,
            EventFormId = eventId,
        };

        await _fieldsService.SaveAsync(field);
        var res = DtoConverter.DynamicFieldToResponse(field);
        return Ok(res);
    }
    
    [HttpPut("{fieldId:guid}")]
    [ProducesResponseType(typeof(FormFieldResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseStatusResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> EditField([FromRoute] Guid eventId, [FromRoute] Guid fieldId,
        [FromBody] UpdateFieldRequest request)
    {
        var check = await _controllerUtils.CheckUserCanEditEventAsync(HttpContext, eventId);
        if (!check.Success)
        {
            return CustomResults.FailedRequest(check.ErrorMsg);
        }

        var form = (await _formService.GetAsync(new DataQueryParams<EventSignupForm>
        {
            Expression = f => f.EventId == eventId
        }))[0];
        var fields = await _fieldsService.GetAsync(new DataQueryParams<FormDynamicField>
        {
            Expression = w => w.EventFormId == form.Id && w.Id == fieldId
        });
        
        if (fields.Length != 1)
        {
            return CustomResults.FailedRequest("Не было найдено ни одного динамического поля с указанным id.");
        }

        var field = fields[0];
        field.FieldTypeId = request.FieldTypeId;
        field.Title = request.Title;
        field.IsRequired = request.IsRequired;
        field.MaxValue = request.MaxValue;
        field.MaxSymbols = request.MaxSymbols;
        field.MinValue = request.MinValue;
        
        await _fieldsService.SaveAsync(field);
        var res = DtoConverter.DynamicFieldToResponse(field);
        return Ok(res);
    }
    
    [HttpDelete("{fieldId:guid}")]
    [ProducesResponseType(typeof(BaseStatusResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseStatusResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteField([FromRoute] Guid eventId, [FromRoute] Guid fieldId)
    {
        var check = await _controllerUtils.CheckUserCanEditEventAsync(HttpContext, eventId);
        if (!check.Success)
        {
            return CustomResults.FailedRequest(check.ErrorMsg);
        }
        var form = (await _formService.GetAsync(new DataQueryParams<EventSignupForm>
        {
            Expression = f => f.EventId == eventId
        }))[0];
        var fields = await _fieldsService.GetAsync(new DataQueryParams<FormDynamicField>
        {
            Expression = w => w.EventFormId == form.Id && w.Id == fieldId
        });
        
        if (fields.Length != 1)
        {
            return CustomResults.FailedRequest("Не было найдено ни одного динамического поля с указанным id.");
        }

        var field = fields[0];
        var fieldValues = await _fieldValuesService.GetAsync(new DataQueryParams<EntryFieldValue>
        {
            Expression = v => v.DynamicFieldId == field.Id
        });
        
        await _fieldValuesService.RemoveRangeAsync(fieldValues);
        await _fieldsService.TryRemoveAsync(field.Id);
        
        return Ok(new BaseStatusResponse
        {
            Completed = true,
            Status = "Успех",
            Message = "Динамическое поле и все его значения были удалены."
        });
    }
}