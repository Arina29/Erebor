using System.Linq.Expressions;
using Erebor.Infrastructure.Data.BaseModels;
using Microsoft.EntityFrameworkCore.Query;

namespace Erebor.Infrastructure.Repositories;

public interface IRepository<TEntity> where TEntity : class
{
    Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>> predicate);

    Task<IEnumerable<TEntity>> GetAllAsync();

    Task<IEnumerable<TEntity>> GetAllAsync(
        Expression<Func<TEntity, bool>>? predicate,
        params Expression<Func<TEntity, object>>[]? including);

    Task<PagedList<TEntity>> GetPagedAsync(
        int pageSize,
        int page,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Expression<Func<TEntity, bool>>? predicate = null,
        params Expression<Func<TEntity, object>>[]? including
    );

    Task<TEntity> AddAsync(TEntity entity);

    Task<IEnumerable<TEntity>> AddRangeAsync(IEnumerable<TEntity> entities);

    Task<bool> UpdateAsync(TEntity entity);

    Task<int> BulkUpdateAsync(
        Expression<Func<TEntity, bool>> predicate,
        Expression<Func<SetPropertyCalls<TEntity>, SetPropertyCalls<TEntity>>> property);

    Task<bool> DeleteAsync(TEntity entity);

    Task<int> BulkDeleteAsync(Expression<Func<TEntity, bool>> predicate);

}