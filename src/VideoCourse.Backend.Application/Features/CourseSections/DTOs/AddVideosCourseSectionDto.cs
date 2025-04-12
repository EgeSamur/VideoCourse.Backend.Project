namespace VideoCourse.Backend.Application.Features.CourseSections.DTOs;

public class AddVideosCourseSectionDto
{
    public int CourseSectionId { get; set; }
    public List<int> VideoIds { get; set; } = new List<int>();
}

