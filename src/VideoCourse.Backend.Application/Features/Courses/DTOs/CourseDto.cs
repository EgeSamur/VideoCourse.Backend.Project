using VideoCourse.Backend.Application.Features.CourseSections.DTOs;

namespace VideoCourse.Backend.Application.Features.Courses.DTOs;

public class CourseDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string ThumbnailUrl { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
    
    // Varolan bölüm ID'leri
    public List<CourseSectionDto> Sections { get; set; } = new List<CourseSectionDto>();
}
