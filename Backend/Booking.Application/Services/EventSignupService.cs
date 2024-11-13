﻿using Booking.Core.DataQuery;
using Booking.Core.Entities;

namespace Booking.Application.Services;

public class EventSignupService
{
    private readonly BaseService<UserEvent> _eventService;
    private readonly BaseService<EventSignupWindow> _eventSignupWindowService;
    private readonly BaseService<EventSignupForm> _eventFormService;
    private readonly BaseService<FormDynamicField> _formDynamicFieldsService;
    private readonly BaseService<User> _userService;
    private readonly BaseService<EventSignupEntry> _entryService;
    private readonly BaseService<EntryFieldValue> _fieldValueService;

    public EventSignupService(BaseService<UserEvent> eventService, BaseService<EventSignupWindow> eventSignupWindowService,
        BaseService<EventSignupForm> eventFormService, BaseService<FormDynamicField> formDynamicFieldsService, 
        BaseService<User> userService, BaseService<EventSignupEntry> entryService,
        BaseService<EntryFieldValue> fieldValueService)
    {
        _eventService = eventService;
        _eventSignupWindowService = eventSignupWindowService;
        _eventFormService = eventFormService;
        _formDynamicFieldsService = formDynamicFieldsService;
        _userService = userService;
        _entryService = entryService;
        _fieldValueService = fieldValueService;
    }
    
    public async Task<(bool Completed, EventSignupEntry? Entry, string Comment)> SignupUserToEventAsync(Guid userId, Guid eventWindowId, 
        string? phone, string? email, string? fio, Dictionary<Guid, string> dynamicFieldValues)
    {
        var window = (await _eventSignupWindowService.GetAsync(new DataQueryParams<EventSignupWindow>
        {
            Expression = w => w.Id == eventWindowId
        }))[0];
        
        if (window.TicketsLeft <= 0)
        {
            return (false, null, "No places for that time have left.");
        }
        
        var entry = new EventSignupEntry
        {
            Id = Guid.NewGuid(),
            SignupWindowId = eventWindowId,
            UserId = userId,
            Phone = phone,
            Fio = fio,
            Email = email
        };
        await _entryService.SaveAsync(entry);
        foreach (var kvPair in dynamicFieldValues)
        {
            var entryFieldValue = new EntryFieldValue
            {
                Id = Guid.NewGuid(),
                EventSignupEntryId = entry.Id,
                DynamicFieldId = kvPair.Key,
                Value = kvPair.Value
            };
            await _fieldValueService.SaveAsync(entryFieldValue);
        }

        return (true, entry, "User successfully signed up on event.");
    }

    public async Task<(string? Phone, string? Fio, string? Email, Dictionary<string, string> DynamicValues)> GetEntryFormValues(Guid entryId)
    {
        var entry = await _entryService.GetByIdOrDefaultAsync(entryId);
        var values = await _fieldValueService.GetAsync(new DataQueryParams<EntryFieldValue>
        {
            Expression = fieldValue => fieldValue.EventSignupEntryId == entryId,
            IncludeParams = new IncludeParams<EntryFieldValue>
            {
                IncludeProperties = [fieldValue => fieldValue.DynamicField]
            }
        });

        var res = values.ToDictionary(v => v.DynamicField.Title, v => v.Value);
        return (entry.Phone, entry.Fio, entry.Email, res);
    }
    
}