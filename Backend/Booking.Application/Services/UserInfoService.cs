using Booking.Core.DataQuery;
using Booking.Core.Entities;
using Microsoft.AspNetCore.Http;

namespace Booking.Application.Services;

public class UserInfoService
{
    private readonly BaseService<UserEvent> _eventsService;
    private readonly BaseService<User> _userService;

    public UserInfoService(BaseService<UserEvent> eventsService, BaseService<User> userService)
    {
        _eventsService = eventsService;
        _userService = userService;
    }
    
    public async Task<UserEvent[]> GetEventsForProfileAsync(Guid userId, int count)
    {
        return await _eventsService.GetAsync(new DataQueryParams<UserEvent>
        {
            Expression = e => e.CreatorUserId == userId,
            Paging = new PagingParams
            {
                Skip = 0,
                Take = count
            },
            Sorting = new SortingParams<UserEvent>
            {
                OrderBy = e => e.DateStart,
                PropertyName = "DateStart",
                Ascending = true
            }
        });
    }

    public async Task UpdateUserAvatarAsync(User user, string webRootPath, string imagesFolderRelativePath, string defaultAvatarFilename, IFormFile? newFile)
    {
        var imagePath = Path.Combine(webRootPath, user.AvatarImageFilepath);
        if (!string.IsNullOrWhiteSpace(user.AvatarImageFilepath) && !user.AvatarImageFilepath.Contains(defaultAvatarFilename) && File.Exists(imagePath))
        {
            File.Delete(imagePath);
        }

        if (newFile == null)
        {
            user.AvatarImageFilepath = Path.Combine(imagesFolderRelativePath, defaultAvatarFilename);
        }
        else
        {
            var fileExtension = newFile.FileName.Split('.')[^1];
            var fileName = $"{user.Id}.{fileExtension}";
            var fullPath = Path.Combine(webRootPath, imagesFolderRelativePath, fileName);
            await using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                await newFile.CopyToAsync(stream);
            }

            user.AvatarImageFilepath = Path.Combine(imagesFolderRelativePath, fileName);
        }

        await _userService.SaveAsync(user);
    }
}