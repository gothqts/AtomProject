﻿using System.Linq.Expressions;

namespace Booking.Core.DataQuery;

public class SortingParams<TEntity> where TEntity : class
{
    public Expression<Func<TEntity, object?>>? OrderBy { get; set; } = null!;
    
    public Expression<Func<TEntity, object?>>? ThenBy { get; set; } = null!;
    
    public string? PropertyName { get; set; }

    public bool Ascending { get; set; } = true;
}