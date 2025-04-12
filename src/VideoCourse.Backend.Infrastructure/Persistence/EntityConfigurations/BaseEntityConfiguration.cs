using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VideoCourse.Backend.Shared.Domain.Entities;

namespace VideoCourse.Backend.Infrastructure.Persistence.EntityConfigurations;

public abstract class BaseEntityConfiguration<TEntity> : IEntityTypeConfiguration<TEntity> where TEntity : BaseEntity
{
    public virtual void Configure(EntityTypeBuilder<TEntity> builder)
    {
        builder.HasKey(e => e.Id);
        
        builder.Property(e => e.Id).HasColumnName("id").IsRequired();
        
        builder.Property(e => e.IsDeleted).HasColumnName("is_deleted").IsRequired();
        builder.Property(e => e.CreatedDate).HasColumnName("created_date").IsRequired();
        builder.Property(e => e.UpdatedDate).HasColumnName("updated_date");
        builder.Property(e => e.CreatedUserId).HasColumnName("created_user_id");
        builder.Property(e => e.UpdatedUserId).HasColumnName("updated_user_id");
        
        builder.HasQueryFilter(e => !e.IsDeleted);
    }
}
