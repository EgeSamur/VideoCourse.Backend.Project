using VideoCourse.Backend.Shared.Domain.Entities;

namespace VideoCourse.Backend.Domain.Entities;

public class CourseCourseSection : BaseEntity
{
    public int CourseId { get; set; }
    public int CourseSectionId { get; set; }
    public int OrderIndex { get; set; }  // Kurs içindeki bölüm sıralaması

    // Navigation properties
    public Course Course { get; set; }
    //CourseSection
    public CourseSection Section { get; set; }
}