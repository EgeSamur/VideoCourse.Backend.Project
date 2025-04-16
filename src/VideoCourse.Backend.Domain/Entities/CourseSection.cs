using VideoCourse.Backend.Shared.Domain.Entities;

namespace VideoCourse.Backend.Domain.Entities;

// CourseSection - Kurs İçi Bölüm entity'si
public class CourseSection : BaseEntity
{
    public string Title { get; set; } = String.Empty;
    public string Description { get; set; } = String.Empty;

    // Navigation properties
    public ICollection<CourseCourseSection> CourseRelations { get; set; } 
    public ICollection<CourseSectionVideo> CourseSectionVideos { get; set; }
}
