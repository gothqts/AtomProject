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
}