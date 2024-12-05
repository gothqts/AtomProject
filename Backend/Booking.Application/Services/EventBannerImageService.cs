using Booking.Core.Entities;
using Microsoft.AspNetCore.Http;

namespace Booking.Application.Services;

public class EventBannerImageService
{
    public readonly string EventImagesRelativePath = Path.Combine("images", "event-banners");
    public readonly string DefaultEventImageFilename = "default-banner.jpg";
    
    private readonly BaseService<UserEvent> _eventsService;

    public EventBannerImageService(BaseService<UserEvent> eventsService)
    {
        _eventsService = eventsService;
    }
    
    public async Task UpdateEventImageAsync(UserEvent userEvent, string webRootPath, IFormFile? newFile)
    {
        var folder = Path.Combine(webRootPath, EventImagesRelativePath);
        if (!Directory.Exists(folder))
        {
            Directory.CreateDirectory(folder);
        }
        
        if (!string.IsNullOrWhiteSpace(userEvent.BannerImageFilepath) && !userEvent.BannerImageFilepath.Contains(DefaultEventImageFilename))
        {
            var imagePath = Path.Combine(webRootPath, userEvent.BannerImageFilepath);
            if (File.Exists(imagePath))
            {
                File.Exists(imagePath);
            }
        }

        if (newFile == null)
        {
            userEvent.BannerImageFilepath = Path.Combine(EventImagesRelativePath, DefaultEventImageFilename);
        }
        else
        {
            var fileExtension = newFile.FileName.Split('.')[^1];
            var fileName = $"{userEvent.Id}.{fileExtension}";
            var fullPath = Path.Combine(webRootPath, EventImagesRelativePath, fileName);
            await using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                await newFile.CopyToAsync(stream);
            }

            userEvent.BannerImageFilepath = Path.Combine(EventImagesRelativePath, fileName);
        }

        await _eventsService.SaveAsync(userEvent);
    }
}