namespace VideoCourse.Backend.Application.Features.CourseSections.DTOs;

public class SwapSectionVideosDto
{
    public int CourseSectionId { get; set; }
    public int FirstVideoId { get; set; }
    public int SecondVideoId { get; set; }
}