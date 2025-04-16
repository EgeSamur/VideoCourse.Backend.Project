using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VideoCourse.Backend.Domain.Entities;

namespace VideoCourse.Backend.Infrastructure.Persistence.EntityConfigurations;

public class CourseCourseSectionConfiguration : BaseEntityConfiguration<CourseCourseSection>
{
    public override void Configure(EntityTypeBuilder<CourseCourseSection> builder)
    {
        base.Configure(builder);
        builder.ToTable("course_course_sections");
        builder.Property(csr => csr.CourseId).HasColumnName("course_id").IsRequired();
        builder.Property(csr => csr.CourseSectionId).HasColumnName("course_section_id").IsRequired();
        builder.Property(csr => csr.OrderIndex).HasColumnName("order_index").IsRequired();

        //// Aynı kurs içinde aynı sıra numarasına sahip iki bölüm olamaz
        //builder.HasIndex(csr => new { csr.CourseId, csr.OrderIndex }).IsUnique();

        //// Aynı kursda aynı bölüm yalnızca bir kez kullanılabilir
        //builder.HasIndex(csr => new { csr.CourseId, csr.CourseSectionId }).IsUnique();
    }
}
