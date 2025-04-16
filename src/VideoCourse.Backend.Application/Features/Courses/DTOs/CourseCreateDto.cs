namespace VideoCourse.Backend.Application.Features.Courses.DTOs;

public class CourseCreateDto
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string ThumbnailUrl { get; set; } = string.Empty;
    // Varolan bölüm ID'leri
    //public List<CourseSectionCreateDto> Sections { get; set; } = new List<CourseSectionCreateDto>();
}
