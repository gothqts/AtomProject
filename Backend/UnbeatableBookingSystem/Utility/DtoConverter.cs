using Booking.Core.Entities;
using UnbeatableBookingSystem.Controllers.Base.Responses;

namespace UnbeatableBookingSystem.Utility;

public static class DtoConverter
{
    public static BasicEventInfo ConvertEventToBasicInfo(UserEvent userEvent)
    {
        return new BasicEventInfo
        {
            Id = userEvent.Id,
            Title = userEvent.Title,
            BannerImage = userEvent.PathToBannerImage,
            DateStart = userEvent.DateStart,
            DateEnd = userEvent.DateEnd,
            IsOnline = userEvent.IsOnline,
            City = userEvent.City,
            Address = userEvent.Address,
            IsSignupOpen = userEvent.IsSignupOpened
        };
    }
}