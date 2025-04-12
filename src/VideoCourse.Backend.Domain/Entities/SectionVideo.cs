using VideoCourse.Backend.Shared.Domain.Entities;

namespace VideoCourse.Backend.Domain.Entities;

// SectionVideo - Bölüm ve video arasındaki ilişki
public class SectionVideo : BaseEntity
{
    public int CourseSectionId { get; set; }
    public int VideoId { get; set; }
    public int OrderIndex { get; set; }  // Bölüm içindeki video sıralaması

    // Navigation properties
    public CourseSection CourseSection { get; set; } = new CourseSection();
    public Video Video { get; set; } = new Video();
}