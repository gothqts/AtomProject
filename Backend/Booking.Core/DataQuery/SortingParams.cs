using System.Linq.Expressions;
using Booking.Core.Interfaces;

namespace Booking.Core.DataQuery;

public class SortingParams<TEntity> where TEntity : class, IHasId, new()
{
    public Expression<Func<TEntity, object?>> OrderBy { get; set; } = null!;
    
    public string? PropertyName { get; set; }

    public bool Ascending { get; set; } = true;
}