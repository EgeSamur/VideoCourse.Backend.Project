using VideoCourse.Backend.Application.Features.Videos.DTOs;

namespace VideoCourse.Backend.Application.Features.CourseSections.DTOs;

public class CourseSectionDto
{
    public int CourseId { get; set; }
    public string Title { get; set; } = String.Empty;
    public string Description { get; set; } = String.Empty;
    public int OrderIndex { get; set; } // Kurs içindeki sıralama
    public ICollection<VideoDto> Videos { get; set; } = new List<VideoDto>();
}  