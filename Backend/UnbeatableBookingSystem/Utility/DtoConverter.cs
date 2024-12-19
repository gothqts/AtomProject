using Booking.Core.Entities;
using UnbeatableBookingSystem.Controllers.Base.Responses;
using UnbeatableBookingSystem.Controllers.Events.Responses;
using UnbeatableBookingSystem.Controllers.UserActions.Responses;

namespace UnbeatableBookingSystem.Utility;

public static class DtoConverter
{
    public static BasicEventInfoResponse ConvertEventToBasicInfo(UserEvent userEvent, string imagesRelativePath, 
        string defaultImageFilename, HttpRequest request)
    {
        return new BasicEventInfoResponse
        {
            Id = userEvent.Id,
            Title = userEvent.Title,
            BannerImage = GetEventImageUrl(userEvent, imagesRelativePath, defaultImageFilename, request),
            DateStart = userEvent.DateStart,
            DateEnd = userEvent.DateEnd,
            IsOnline = userEvent.IsOnline,
            City = userEvent.City,
            Address = userEvent.Address,
            IsSignupOpened = userEvent.IsSignupOpened,
            IsPublic = userEvent.IsPublic,
            Description = userEvent.Description
        };
    }

    public static string GetAvatarUrl(User user, string imagesRelativePath, string defaultAvatarFilename, HttpRequest request)
    {
        var avatarPath = string.IsNullOrWhiteSpace(user.AvatarImageFilepath)
            ? Path.Combine(imagesRelativePath, defaultAvatarFilename)
            : user.AvatarImageFilepath;
        var splitted = avatarPath.Split(Path.DirectorySeparatorChar);
        var relativeUrl = string.Join('/', splitted);
        return $"{request.Scheme}://{request.Host}/{relativeUrl}";
    }

    public static string GetEventImageUrl(UserEvent userEvent, string imagesRelativePath, string defaultImageFilename, 
        HttpRequest request)
    {
        var avatarPath = string.IsNullOrWhiteSpace(userEvent.BannerImageFilepath)
            ? Path.Combine(imagesRelativePath, defaultImageFilename)
            : userEvent.BannerImageFilepath;
        var splitted = avatarPath.Split(Path.DirectorySeparatorChar);
        var relativeUrl = string.Join('/', splitted);
        return $"{request.Scheme}://{request.Host}/{relativeUrl}";
    }
    
    public static ContactsResponse OrganizerContactsToResponse(OrganizerContacts contacts)
    {
        return new ContactsResponse
        {
            Email = contacts.Email,
            Fio = contacts.Fio,
            Phone = contacts.Phone,
            Telegram = contacts.Telegram,
            Id = contacts.Id
        };
    }

    public static SignupWindowResponse SignupWindowToResponse(EventSignupWindow window)
    {
        return new SignupWindowResponse
        {
            Title = window.Title,
            MaxVisitors = window.MaxVisitors,
            TicketsLeft = window.TicketsLeft,
            Id = window.Id,
            DateTime = new DateTime(window.Date, window.Time, DateTimeKind.Utc)
        };
    }

    public static FormFieldResponse DynamicFieldToResponse(FormDynamicField field)
    {
        return new FormFieldResponse
        {
            Id = field.Id,
            Title = field.Title,
            IsRequired = field.IsRequired,
            Type = field.FieldType?.Title,
            MaxSymbols = field.MaxSymbols,
            MinValue = field.MinValue,
            MaxValue = field.MaxValue
        };
    }

    public static EventResponse CreateEventResponse(UserEvent userEvent, EventSignupWindow[] windows,
        EventSignupForm form, FormDynamicField[] fields, OrganizerContacts[] contacts, 
        string imagesRelativePath, string defaultImageFilename, HttpRequest request)
    {
        return new EventResponse
        {
            Id = userEvent.Id,
            CreationDate = userEvent.CreationDate,
            IsPublic = userEvent.IsPublic,
            Title = userEvent.Title,
            BannerImage = GetEventImageUrl(userEvent, imagesRelativePath, defaultImageFilename, request),
            IsOnline = userEvent.IsOnline,
            IsSignupOpened = userEvent.IsSignupOpened,
            City = userEvent.City,
            Address = userEvent.Address,
            DateStart = userEvent.DateStart,
            DateEnd = userEvent.DateEnd,
            Description = userEvent.Description,
            SignupWindows = windows.Select(SignupWindowToResponse).ToArray(),
            SignupForm = new EventFormResponse
            {
                IsFioRequired = form.IsFioRequired,
                IsPhoneRequired = form.IsPhoneRequired,
                IsEmailRequired = form.IsEmailRequired,
                DynamicFields = fields.Select(DynamicFieldToResponse).ToArray()
            },
            Contacts = contacts.Select(OrganizerContactsToResponse).ToArray()
        };
    }
}