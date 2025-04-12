﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VideoCourse.Backend.Domain.Entities;

namespace VideoCourse.Backend.Infrastructure.Persistence.EntityConfigurations;

// CourseSection Konfigürasyonu
public class CourseSectionConfiguration : BaseEntityConfiguration<CourseSection>
{
    public override void Configure(EntityTypeBuilder<CourseSection> builder)
    {
        base.Configure(builder);
        builder.ToTable("course_sections");
        builder.Property(cs => cs.CourseId).HasColumnName("course_id").IsRequired();
        builder.Property(cs => cs.Title).HasColumnName("title").IsRequired();
        builder.Property(cs => cs.Description).HasColumnName("description").IsRequired();
        builder.Property(cs => cs.OrderIndex).HasColumnName("order_index").IsRequired();

        builder.HasOne(cs => cs.Course)
            .WithMany(c => c.Sections)
            .HasForeignKey(cs => cs.CourseId);

    }
}
