using VideoCourse.Backend.Shared.Domain.Entities;

namespace VideoCourse.Backend.Shared.Persistence.Abstraction;

public interface IWriteRepository<TEntity> : IQuery<TEntity>
    where TEntity : BaseEntity
{
    Task AddAsync(TEntity entity);

    Task AddRangeAsync(ICollection<TEntity> entity);

    Task UpdateAsync(TEntity entity);

    Task UpdateRangeAsync(ICollection<TEntity> entity);

    Task DeleteAsync(TEntity entity);

    Task DeleteRangeAsync(ICollection<TEntity> entity);
    Task HardDeleteAsync(TEntity entity);
    Task HardDeleteRangeAsync(ICollection<TEntity> entities);
}