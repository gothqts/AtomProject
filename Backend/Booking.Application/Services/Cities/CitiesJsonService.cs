using System.Text.Json;
using Booking.Application.Services.Cities.Models;
using System.Linq.Dynamic.Core;

namespace Booking.Application.Services.Cities;

public class CitiesJsonService : ICitiesService
{
    private FullCityModel[] _allCities;
    
    public CitiesJsonService()
    {
        var pathToFile = Path.Combine(Environment.CurrentDirectory, "russian-cities.json");
        var jsonContent = File.ReadAllText(pathToFile);
        var cities = JsonSerializer.Deserialize<FullCityModel[]>(jsonContent);
        _allCities = cities ?? [];
    }
    
    public Task<FullCityModel[]> GetFullCitiesInfo(CityQuery queryParams)
    {
        var set = _allCities.AsQueryable();
        if (queryParams.Expression != null)
        {
            set = set.Where(queryParams.Expression);
        }
        if (queryParams.Filters != null)
        {
            foreach (var filter in queryParams.Filters)
            {
                set = set.Where(filter);
            }
        }
        
        if (queryParams.Sorting != null)
        {
            if (queryParams.Sorting.OrderBy == null)
            {
                if (queryParams.Sorting.PropertyName != null)
                {
                    set = queryParams.Sorting.Ascending ? set.OrderBy(queryParams.Sorting.PropertyName) : 
                        set.OrderBy(queryParams.Sorting.PropertyName + " descending");
                }
            }
            else
            {
                if (queryParams.Sorting.ThenBy != null)
                {
                    set = queryParams.Sorting.Ascending ? 
                        set.OrderBy(queryParams.Sorting.OrderBy).ThenBy(queryParams.Sorting.ThenBy) : 
                        set.OrderByDescending(queryParams.Sorting.OrderBy).ThenByDescending(queryParams.Sorting.ThenBy);
                }
                else
                {
                    set = queryParams.Sorting.Ascending ? 
                        set.OrderBy(queryParams.Sorting.OrderBy) : 
                        set.OrderByDescending(queryParams.Sorting.OrderBy);
                }
            }
        }
        
        if (queryParams.Paging != null)
        {
            set = set.Skip(queryParams.Paging.Skip).Take(queryParams.Paging.Take);
        }
        
        return Task.FromResult(set.ToArray());
    }

    public async Task<ShortCityModel[]> GetShortCitiesInfo(CityQuery queryParams)
    {
        var fullCities = await GetFullCitiesInfo(queryParams);
        return fullCities.Select(c => new ShortCityModel
        {
            Population = c.Population,
            Name = c.Name
        }).ToArray();
    }
}