using Booking.Core.DataQuery;
using Booking.Core.Interfaces;
using Booking.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;

namespace Booking.Application.Services;

public class BaseService<TEntity> where TEntity : class, IHasId
{
    private readonly IDbContextFactory<BookingDbContext> _dbContextFactory;

    public BaseService(IDbContextFactory<BookingDbContext> dbContextFactory)
    {
        _dbContextFactory = dbContextFactory;
    }
    
    /// <summary>
    /// Выполнить запрос на получение объектов к базе данных с указанным параметрами запроса.
    /// </summary>
    /// <returns>
    /// Возвращается массив объектов, которые соответствуют запросу.
    /// </returns>
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
        
        if (queryParams.IncludeParams != null)
        {
            if (queryParams.IncludeParams.IncludeProperties != null)
            {
                foreach (var propertyPath in queryParams.IncludeParams.IncludeProperties)
                {
                    set = set.Include(propertyPath);
                }
            }

            if (queryParams.IncludeParams.IncludePropertiesPaths != null)
            {
                foreach (var propertyPath in queryParams.IncludeParams.IncludePropertiesPaths)
                {
                    set = set.Include(propertyPath);
                }
            }
        }
        
        return set.ToArray();
    }

    /// <summary>
    /// Получить объект из базы данных по конкретному идентификатору id
    /// </summary>
    /// <param name="id">Идентификатор объекта в базе данных.</param>
    /// <returns>
    /// Возвращается объект с указанным id, если он был найден в базе данных. Иначе - null.
    /// </returns>
    public virtual async Task<TEntity?> GetByIdOrDefaultAsync(Guid id)
    {
        await using var dbContext = await _dbContextFactory.CreateDbContextAsync();
        var set = dbContext.Set<TEntity>();
        return set.FirstOrDefault(item => item.Id == id);
    }
    
    /// <summary>
    /// Удаляет сущность с указанным id из базы данных, если она существует.
    /// </summary>
    /// <param name="id">Идентификатор объекта в базе данных.</param>
    /// <returns>
    /// True - объект был найден и удален из базы данных.
    /// False - объект с указанным id не был найден в базе данных.
    /// </returns>
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

    /// <summary>
    /// Сохраняет сущность в соответствующую таблицу. Обновляет её поля, если она уже существует.
    /// </summary>
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