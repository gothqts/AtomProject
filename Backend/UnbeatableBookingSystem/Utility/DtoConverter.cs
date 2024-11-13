using Booking.Core.Entities;
using UnbeatableBookingSystem.Controllers.Base.Responses;
using UnbeatableBookingSystem.Controllers.UserActions.Responses;

namespace UnbeatableBookingSystem.Utility;

public static class DtoConverter
{
    public static BasicEventInfoResponse ConvertEventToBasicInfo(UserEvent userEvent)
    {
        return new BasicEventInfoResponse
        {
            Id = userEvent.Id,
            Title = userEvent.Title,
            BannerImage = userEvent.BannerImageFilepath,
            DateStart = userEvent.DateStart,
            DateEnd = userEvent.DateEnd,
            IsOnline = userEvent.IsOnline,
            City = userEvent.City,
            Address = userEvent.Address,
            IsSignupOpen = userEvent.IsSignupOpened
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

    public static ContactsResponse OrganizerContactsToResponse(OrganizerContacts contacts)
    {
        return new ContactsResponse
        {
            Email = contacts.Email,
            Fio = contacts.Fio,
            Phone = contacts.Phone,
            Telegram = contacts.Telegram
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
}