using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VideoCourse.Backend.Domain.Entities;

namespace VideoCourse.Backend.Infrastructure.Persistence.EntityConfigurations;

public class VideoProgressConfiguration : BaseEntityConfiguration<VideoProgress>
{
    public override void Configure(EntityTypeBuilder<VideoProgress> builder)
    {
        base.Configure(builder);
        builder.ToTable("video_progresses");
        builder.Property(vp => vp.UserId).HasColumnName("user_id").IsRequired();
        builder.Property(vp => vp.VideoId).HasColumnName("video_id").IsRequired();
        builder.Property(vp => vp.IsCompleted).HasColumnName("is_completed").IsRequired();
        builder.Property(vp => vp.LastWatchedPositionInSeconds).HasColumnName("last_watched_position_in_seconds").IsRequired();
        builder.Property(vp => vp.LastWatchedDate).HasColumnName("last_watched_date").IsRequired();

        builder.HasOne(vp => vp.User)
            .WithMany(u => u.VideoProgresses)
            .HasForeignKey(vp => vp.UserId);

        builder.HasOne(vp => vp.Video)
            .WithMany(v => v.UserProgresses)
            .HasForeignKey(vp => vp.VideoId);
    }
}
