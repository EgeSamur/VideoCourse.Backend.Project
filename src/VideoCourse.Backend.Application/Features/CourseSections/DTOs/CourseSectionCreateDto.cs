namespace VideoCourse.Backend.Application.Features.CourseSections.DTOs;

public class CourseSectionCreateDto
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int OrderIndex { get; set; }
    public List<int> VideoIds { get; set; } = new List<int>();
}
