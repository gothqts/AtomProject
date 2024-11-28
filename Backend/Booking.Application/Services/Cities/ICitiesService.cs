using Booking.Application.Services.Cities.Models;

namespace Booking.Application.Services.Cities;

public interface ICitiesService
{
    public Task<FullCityModel[]> GetFullCitiesInfo(CityQuery queryParams);
    
    public Task<ShortCityModel[]> GetShortCitiesInfo(CityQuery queryParams);
}