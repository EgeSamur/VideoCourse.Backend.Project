using VideoCourse.Backend.Application.Features.Videos.DTOs;

namespace VideoCourse.Backend.Application.Features.CourseSections.DTOs;

public class CourseSectionDto
{
    public int CourseSectionId { get; set; }
    public string Title { get; set; } = String.Empty;
    public string Description { get; set; } = String.Empty;
    public ICollection<VideoDto> Videos { get; set; } = new List<VideoDto>();
}
