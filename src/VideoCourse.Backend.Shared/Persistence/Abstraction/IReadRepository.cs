using System.Linq.Expressions;
using VideoCourse.Backend.Shared.Domain.Entities;
using VideoCourse.Backend.Shared.Utils.Pagination;
using Microsoft.EntityFrameworkCore.Query;

namespace VideoCourse.Backend.Shared.Persistence.Abstraction;

public interface IReadRepository<TEntity> : IQuery<TEntity>
    where TEntity : BaseEntity
{
    
    Task<TEntity?> GetAsync(
        Expression<Func<TEntity, bool>> predicate,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        bool enableTracking = false,
        CancellationToken cancellationToken = default
    );

    Task<IPaginate<TEntity>> GetListAsync(
        Expression<Func<TEntity, bool>>? predicate = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        int index = 0,
        int size = 10,
        bool isAll = false,
        bool enableTracking = false,
        CancellationToken cancellationToken = default
    );
    
    Task<TResult?> GetWithProjectionAsync<TResult>(
        Expression<Func<TEntity, bool>> predicate,
        Expression<Func<TEntity, TResult>> selector,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        bool enableTracking = false,
        CancellationToken cancellationToken = default
    );
    
    Task<IPaginate<TResult>> GetListWithProjectionAsync<TResult>(
        Expression<Func<TEntity, TResult>> selector,
        Expression<Func<TEntity, bool>>? predicate = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        int index = 0,
        int size = 10,
        bool isAll = false,
        bool enableTracking = false,
        CancellationToken cancellationToken = default
    );

    
    Task<bool> AnyAsync(
        Expression<Func<TEntity, bool>>? predicate = null,
        bool enableTracking = false,
        CancellationToken cancellationToken = default
    );
}