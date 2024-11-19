﻿using System.Linq.Expressions;

namespace Booking.Core.DataQuery;

public class IncludeParams<TEntity>
{
    public List<Expression<Func<TEntity, object?>>>? IncludeProperties { get; set; }
    
    public List<string>? IncludePropertiesPaths { get; set; }
}