using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;
using Erebor.Infrastructure.Data.BaseModels;

namespace Erebor.Infrastructure.Repositories;

public class GenericRepository<TEntity> : IRepository<TEntity> where TEntity : class
{
    private readonly DbContext _context;
    private readonly DbSet<TEntity> _table;

    public GenericRepository(DbContext context)
    {
        _context = context;
        _table = _context.Set<TEntity>();
    }

    public virtual async Task<IEnumerable<TEntity>> GetAllAsync() =>
        await _table.AsNoTracking().ToListAsync();

    public virtual async Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>> predicate) =>
        await _table.AsNoTracking().SingleOrDefaultAsync(predicate);

    public virtual async Task<IEnumerable<TEntity>> GetAllAsync(
        Expression<Func<TEntity, bool>>? predicate,
        params Expression<Func<TEntity, object>>[]? including
    )
    {
        var items = _table.AsNoTracking().AsQueryable();
        if (predicate is not null)
        {
            items = items.Where(predicate);
        }
        including?.ToList().ForEach(include =>
        {
            items = items.Include(include);
        });
        return await items.ToListAsync();
    }

    public virtual async Task<PagedList<TEntity>> GetPagedAsync(
        int pageSize,
        int page,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy,
        Expression<Func<TEntity, bool>>? predicate,
        params Expression<Func<TEntity, object>>[]? including
    )
    {
        var items = _table.AsNoTracking().AsQueryable();
        if (predicate is not null)
        {
            items = items.Where(predicate);
        }
        if (orderBy is not null)
        {
            items = orderBy(items);
        }
        including?.ToList().ForEach(include =>
        {
            items = items.Include(include);
        });

        var totalCount = items.Count();

        items = items.Skip((page-1) * pageSize).Take(pageSize);

        return new PagedList<TEntity>
        {
            PageSize = pageSize,
            Page = page,
            TotalCount = totalCount,
            PageCount = (int)Math.Ceiling(totalCount /(double)pageSize),
            Data = await items.ToListAsync()
        };
    }

    public virtual async Task<TEntity> AddAsync(TEntity entity)
    {
        await _context.AddAsync(entity);
        await _context.SaveChangesAsync();

        return entity;
    }

    public virtual async Task<IEnumerable<TEntity>> AddRangeAsync(IEnumerable<TEntity> entities)
    {
        var entitiesToAdd = entities.ToList();
        await _context.AddRangeAsync(entitiesToAdd);
        await _context.SaveChangesAsync();

        return entitiesToAdd;
    }

    public virtual async Task<bool> UpdateAsync(TEntity entity)
    {
        _context.Update(entity);
        return await SaveAsync();
    }
    public virtual async Task<int> BulkUpdateAsync(
        Expression<Func<TEntity, bool>> predicate,
        Expression<Func<SetPropertyCalls<TEntity>, SetPropertyCalls<TEntity>>> property)
    {
        return await _table.Where(predicate).ExecuteUpdateAsync(property);
    }

    public virtual async Task<bool> DeleteAsync(TEntity entity)
    {
        _context.Remove(entity);
        return await SaveAsync();
    }

    public virtual async Task<int> BulkDeleteAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return await _table.Where(predicate).ExecuteDeleteAsync();
    }


    private async Task<bool> SaveAsync()
    {
        try
        {
            await _context.SaveChangesAsync();
            return true;
        }
        catch
        {
            return false;
        }
    }

}