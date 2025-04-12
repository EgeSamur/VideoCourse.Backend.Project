using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VideoCourse.Backend.Domain.Entities;

namespace VideoCourse.Backend.Infrastructure.Persistence.EntityConfigurations;

// Video Konfigürasyonu 
public class VideoConfiguration : BaseEntityConfiguration<Video>
{
    public override void Configure(EntityTypeBuilder<Video> builder)
    {
        base.Configure(builder);
        builder.ToTable("videos");
        builder.Property(v => v.Title).HasColumnName("title").IsRequired();
        builder.Property(v => v.Description).HasColumnName("description").IsRequired();
        builder.Property(v => v.VideoUrl).HasColumnName("video_url").IsRequired();
        builder.Property(v => v.DurationInSeconds).HasColumnName("duration_in_seconds").IsRequired();
        builder.Property(v => v.ThumbnailUrl).HasColumnName("thumbnail_url").IsRequired();
        builder.HasIndex(u => u.Title).IsUnique();
    }
}
