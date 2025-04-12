using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VideoCourse.Backend.Domain.Entities;

namespace VideoCourse.Backend.Infrastructure.Persistence.EntityConfigurations;

public class UserCourseConfiguration : BaseEntityConfiguration<UserCourse>
{
    public override void Configure(EntityTypeBuilder<UserCourse> builder)
    {
        base.Configure(builder);
        builder.ToTable("user_courses");
        builder.Property(uc => uc.UserId).HasColumnName("user_id").IsRequired();
        builder.Property(uc => uc.CourseId).HasColumnName("course_id").IsRequired();
        builder.Property(uc => uc.EnrollmentDate).HasColumnName("enrollment_date").IsRequired();
        builder.Property(uc => uc.IsCompleted).HasColumnName("is_completed").IsRequired();

        builder.HasOne(uc => uc.User)
            .WithMany(u => u.EnrolledCourses)
            .HasForeignKey(uc => uc.UserId);

        builder.HasOne(uc => uc.Course)
            .WithMany(c => c.EnrolledUsers)
            .HasForeignKey(uc => uc.CourseId);
    }
}
