namespace VideoCourse.Backend.Application.Features.Courses.DTOs;

public class AddSectionsToCoursDto
{
    public int CourseId { get; set; }
    public List<int> SectionIds { get; set; } = new List<int>();
}