using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VideoCourse.Backend.Domain.Entities;

namespace VideoCourse.Backend.Infrastructure.Persistence.EntityConfigurations;

// Course Konfigürasyonu
public class CourseConfiguration : BaseEntityConfiguration<Course>
{
    public override void Configure(EntityTypeBuilder<Course> builder)
    {
        base.Configure(builder);
        builder.ToTable("courses");
        builder.Property(c => c.Title).HasColumnName("title").IsRequired();
        builder.Property(c => c.Description).HasColumnName("description").IsRequired();
        builder.Property(c => c.ThumbnailUrl).HasColumnName("thumbnail_url").IsRequired();
        builder.Property(c => c.IsActive).HasColumnName("is_active").IsRequired();
        builder.HasIndex(u => u.Title).IsUnique();
    }
}
