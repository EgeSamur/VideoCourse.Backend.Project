using VideoCourse.Backend.Shared.Domain.Entities;

namespace VideoCourse.Backend.Domain.Entities;

// Course - Kurs entity'si
public class Course : BaseEntity
{
    public string Title { get; set; } = String.Empty;
    public string Description { get; set; } = String.Empty;
    public string ThumbnailUrl { get; set; } = String.Empty;
    public bool IsActive { get; set; } = true;

    // Navigation properties
    public ICollection<CourseCourseSection> CourseCourseSections { get; set; }
    public ICollection<UserCourse> EnrolledUsers { get; set; } 
}
