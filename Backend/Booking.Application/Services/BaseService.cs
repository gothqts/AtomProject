using Booking.Core.DataQuery;
using Booking.Core.Interfaces;
using Booking.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Booking.Application.Services;

public class BaseService<TEntity> where TEntity : class, IHasId, new()
{
    private readonly IDbContextFactory<BookingDbContext> _dbContextFactory;

    public BaseService(IDbContextFactory<BookingDbContext> dbContextFactory)
    {
        _dbContextFactory = dbContextFactory;
    }
    
    public virtual async Task<TEntity[]> GetAsync(DataQueryParams<TEntity> queryParams)
    {
        await using var dbContext = await _dbContextFactory.CreateDbContextAsync();
        var set = dbContext.Set<TEntity>().AsQueryable();
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
            set = queryParams.Sorting.Ascending ? set.OrderBy(queryParams.Sorting.OrderBy) : set.OrderByDescending(queryParams.Sorting.OrderBy);
        }
        
        if (queryParams.Paging != null)
        {
            set = set.Skip(queryParams.Paging.Skip).Take(queryParams.Paging.Take);
        }
        
        return set.ToArray();
    }

    public virtual async Task<TEntity?> GetByIdOrDefaultAsync(Guid id)
    {
        await using var dbContext = await _dbContextFactory.CreateDbContextAsync();
        var set = dbContext.Set<TEntity>();
        return set.FirstOrDefault(item => item.Id == id);
    }
    
    public virtual async Task<bool> TryRemoveAsync(Guid id)
    {
        await using var dbContext = await _dbContextFactory.CreateDbContextAsync();
        var set = dbContext.Set<TEntity>();
        var item = set.FirstOrDefault(item => item.Id == id);
        if (item == null)
        {
            return false;
        }

        set.Remove(item);
        await dbContext.SaveChangesAsync();
        return true;
    }

    public virtual async Task<Guid> SaveAsync(TEntity entity)
    {
        await using var dbContext = await _dbContextFactory.CreateDbContextAsync();
        var set = dbContext.Set<TEntity>();
        var existingItem = set.FirstOrDefault(item => item.Id == entity.Id);
        if (existingItem != null)
        {
            set.Remove(existingItem);
        }

        set.Add(entity);

        await dbContext.SaveChangesAsync();
        return entity.Id;
    }
}