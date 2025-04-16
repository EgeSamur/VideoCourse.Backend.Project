using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VideoCourse.Backend.Domain.Entities;

namespace VideoCourse.Backend.Infrastructure.Persistence.EntityConfigurations;

// SectionVideo Konfigürasyonu
public class CourseSectionVideoConfiguration : BaseEntityConfiguration<CourseSectionVideo>
{
    public override void Configure(EntityTypeBuilder<CourseSectionVideo> builder)
    {
        base.Configure(builder);
        builder.ToTable("course_section_videos");
        builder.Property(sv => sv.CourseSectionId).HasColumnName("course_section_id").IsRequired();
        builder.Property(sv => sv.VideoId).HasColumnName("video_id").IsRequired();
        builder.Property(sv => sv.OrderIndex).HasColumnName("order_index").IsRequired();
    }
}
