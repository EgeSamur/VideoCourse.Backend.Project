using System.Linq.Expressions;
using VideoCourse.Backend.Shared.CrossCuttingConcerns.Exceptions.Types;
using VideoCourse.Backend.Shared.Domain.Entities;
using VideoCourse.Backend.Shared.Persistence.Abstraction;
using VideoCourse.Backend.Shared.Utils.Pagination;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace VideoCourse.Backend.Shared.Persistence.EfCore;

public class RepositoryBase<TEntity, TContext> : IReadRepository<TEntity>, IWriteRepository<TEntity>
    where TEntity : BaseEntity
    where TContext : DbContext
{
    protected readonly TContext Context;

    public RepositoryBase(TContext context)
    {
        Context = context;
    }

    public IQueryable<TEntity> Query()
    {
        return Context.Set<TEntity>();
    }

    public async Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>> predicate, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null, bool enableTracking = false, 
        CancellationToken cancellationToken = default)
    {
        IQueryable<TEntity> queryable = Query();
        if (!enableTracking)
            queryable = queryable.AsNoTracking();
        if (include != null)
            queryable = include(queryable);
        var result = await queryable.FirstOrDefaultAsync(predicate, cancellationToken);
        return result;
    }

    public async Task<IPaginate<TEntity>> GetListAsync(Expression<Func<TEntity, bool>>? predicate = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null, int index = 0,
        int size = 10, bool isAll = false, bool enableTracking = false, CancellationToken cancellationToken = default)
    {
        IQueryable<TEntity> queryable = Query();
        if (index == 0 && size == 0)
            size = int.MaxValue;
        if (isAll)
            size = int.MaxValue;
        if (!enableTracking)
            queryable = queryable.AsNoTracking();
        if (include != null)
            queryable = include(queryable);
        if (predicate != null)
            queryable = queryable.Where(predicate);
        if (orderBy != null)
            return await orderBy(queryable).ToPaginateAsync(index, size, from: 0, cancellationToken);
        return await queryable.ToPaginateAsync(index, size, from: 0, cancellationToken);
    }

    public async Task<TResult?> GetWithProjectionAsync<TResult>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TResult>> selector, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null, 
        bool enableTracking = false,
        CancellationToken cancellationToken = default)
    {
        IQueryable<TEntity> queryable = Query();
        if (!enableTracking)
            queryable = queryable.AsNoTracking();
        if (include != null)
            queryable = include(queryable);
        queryable = queryable.Where(predicate);
        var result = await queryable.Select(selector).FirstOrDefaultAsync(cancellationToken);
        return result;
    }

    public async Task<IPaginate<TResult>> GetListWithProjectionAsync<TResult>(Expression<Func<TEntity, TResult>> selector, Expression<Func<TEntity, bool>>? predicate = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null, int index = 0, int size = 10, bool isAll = false, bool enableTracking = false,
        CancellationToken cancellationToken = default)
    {
        IQueryable<TEntity> queryable = Query();
        if (index == 0 && size == 0)
            size = int.MaxValue;
        if (isAll)
            size = int.MaxValue;
        if (!enableTracking)
            queryable = queryable.AsNoTracking();
        if (include != null)
            queryable = include(queryable);
        if (predicate != null)
            queryable = queryable.Where(predicate);
        if (orderBy != null)
            queryable = orderBy(queryable);
        
        return await queryable.Select(selector).ToPaginateAsync(index, size, 0, cancellationToken);
    }


    public Task<bool> AnyAsync(Expression<Func<TEntity, bool>>? predicate = null, bool enableTracking = false, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public async Task AddAsync(TEntity entity)
    {
        await Context.AddAsync(entity);
    }

    public async Task AddRangeAsync(ICollection<TEntity> entities)
    {
        await Context.AddRangeAsync(entities);
    }


    public Task UpdateAsync(TEntity entity)
    {
        Context.Update(entity);
        return Task.CompletedTask;  
    }

    public Task UpdateRangeAsync(ICollection<TEntity> entities)
    {
        Context.UpdateRange(entities);
        return Task.CompletedTask; 
    }

    public Task DeleteAsync(TEntity entity)
    {
        entity.IsDeleted = true;
        return UpdateAsync(entity);
    }

    public Task DeleteRangeAsync(ICollection<TEntity> entities)
    {
        foreach (var entity in entities)
            entity.IsDeleted = true;
        
        return UpdateRangeAsync(entities);
    }
    public Task HardDeleteAsync(TEntity entity)
    {
        Context.Set<TEntity>().Remove(entity);
        return Context.SaveChangesAsync();
    }

    public Task HardDeleteRangeAsync(ICollection<TEntity> entities)
    {
        Context.Set<TEntity>().RemoveRange(entities);
        return Context.SaveChangesAsync();
    }
}