using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VideoCourse.Backend.Domain.Entities;

namespace VideoCourse.Backend.Infrastructure.Persistence.EntityConfigurations;

// SectionVideo Konfigürasyonu
public class SectionVideoConfiguration : BaseEntityConfiguration<SectionVideo>
{
    public override void Configure(EntityTypeBuilder<SectionVideo> builder)
    {
        base.Configure(builder);
        builder.ToTable("section_videos");
        builder.Property(sv => sv.CourseSectionId).HasColumnName("course_section_id").IsRequired();
        builder.Property(sv => sv.VideoId).HasColumnName("video_id").IsRequired();
        builder.Property(sv => sv.OrderIndex).HasColumnName("order_index").IsRequired();

        builder.HasOne(sv => sv.CourseSection)
            .WithMany(cs => cs.SectionVideos)
            .HasForeignKey(sv => sv.CourseSectionId);

        builder.HasOne(sv => sv.Video)
            .WithMany(v => v.VideoSections)
            .HasForeignKey(sv => sv.VideoId);
    }
}
