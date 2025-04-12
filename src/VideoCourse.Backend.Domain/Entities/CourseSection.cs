using VideoCourse.Backend.Shared.Domain.Entities;

namespace VideoCourse.Backend.Domain.Entities;

// CourseSection - Kurs İçi Bölüm entity'si
public class CourseSection : BaseEntity
{
    public int CourseId { get; set; }
    public string Title { get; set; } = String.Empty;
    public string Description { get; set; } = String.Empty;
    public int OrderIndex { get; set; } // Kurs içindeki sıralama

    // Navigation properties
    public Course Course { get; set; } = new Course();
    public ICollection<SectionVideo> SectionVideos { get; set; } = new List<SectionVideo>();
}
