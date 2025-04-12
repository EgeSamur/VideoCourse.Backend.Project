namespace VideoCourse.Backend.Application.Features.CourseSections.DTOs;

public class DeleteVideosFromCourseSectionDto
{
    public int CourseSectionId { get; set; }
    public List<int> VideoIds { get; set; } = new List<int>();
}