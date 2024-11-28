using System.Linq.Expressions;
using Booking.Core.DataQuery;

namespace Booking.Application.Services.Cities.Models;

public class CityQuery
{
    public Expression<Func<FullCityModel, bool>>? Expression { get; set; }
    
    public PagingParams? Paging { get; set; }
    
    public SortingParams<FullCityModel>? Sorting { get; set; }
    
    public List<Expression<Func<FullCityModel, bool>>>? Filters { get; set; }
}