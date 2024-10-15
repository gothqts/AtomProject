using System.Linq.Expressions;
using Booking.Core.Interfaces;

namespace Booking.Core.DataQuery;

public class DataQueryParams<TEntity> where TEntity : class, IHasId, new()
{
    public Expression<Func<TEntity, bool>>? Expression { get; set; }
    
    public PagingParams? Paging { get; set; }
    
    public SortingParams<TEntity>? Sorting { get; set; }
    
    public List<Expression<Func<TEntity, bool>>>? Filters { get; set; }
}