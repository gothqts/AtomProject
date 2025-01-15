using Booking.Core.DataQuery;
using Booking.Core.Entities;

namespace Booking.Application.Services;

public class EventSignupService
{
    private readonly BaseService<UserEvent> _eventService;
    private readonly BaseService<EventSignupWindow> _eventSignupWindowService;
    private readonly BaseService<EventSignupEntry> _entryService;
    private readonly BaseService<EntryFieldValue> _fieldValueService;

    public EventSignupService(BaseService<UserEvent> eventService, BaseService<EventSignupWindow> eventSignupWindowService, 
        BaseService<EventSignupEntry> entryService, BaseService<EntryFieldValue> fieldValueService)
    {
        _eventService = eventService;
        _eventSignupWindowService = eventSignupWindowService;
        _entryService = entryService;
        _fieldValueService = fieldValueService;
    }
    
    public async Task<(bool Completed, EventSignupEntry? Entry, string Comment)> SignupUserToEventAsync(Guid? userId, Guid eventWindowId, 
        string? phone, string? email, string? fio, Dictionary<Guid, string> dynamicFieldValues)
    {
        var windows = await _eventSignupWindowService.GetAsync(new DataQueryParams<EventSignupWindow>
        {
            Expression = w => w.Id == eventWindowId
        });
        if (windows.Length < 1)
        {
            return (false, null, $"Ошибка: нет окна записи с указанным eventWindowId {eventWindowId}");
        }

        var window = windows[0];
        var userEvent = (await _eventService.GetAsync(new DataQueryParams<UserEvent>
        {
            Expression = e => e.Id == window.EventId
        }))[0];
        
        if (!userEvent.IsSignupOpened)
        {
            return (false, null, "Запись на данное мероприятие закрыта.");
        }
        if (window.TicketsLeft <= 0)
        {
            return (false, null, "Не осталось мест в выбранном окне записи.");
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
        var values = dynamicFieldValues.Select(kvPair => new EntryFieldValue
        {
            Id = Guid.NewGuid(),
            EventSignupEntryId = entry.Id,
            DynamicFieldId = kvPair.Key,
            Value = kvPair.Value
        }).ToArray();
        try
        {
            await _entryService.SaveAsync(entry);
            foreach (var entryFieldValue in values)
            {
                await _fieldValueService.SaveAsync(entryFieldValue);
            }
            window.TicketsLeft -= 1;
            await _eventSignupWindowService.SaveAsync(window);
        }
        catch (Exception e)
        {
            foreach (var entryFieldValue in values)
            {
                await _fieldValueService.TryRemoveAsync(entryFieldValue.Id);
            }
            await _entryService.TryRemoveAsync(entry.Id);
            
            return (false, null, $"Ошибка во время сохранения записи: {e.Message}");
        }
        

        return (true, entry, "Пользователь успешно записан на мероприятие.");
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
        return (entry?.Phone, entry?.Fio, entry?.Email, res);
    }
    
}