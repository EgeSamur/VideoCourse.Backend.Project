namespace VideoCourse.Backend.Application.Features.Courses.DTOs;

public class DeleteSectionsFromCourseDto
{
    public int CourseId { get; set; }
    public List<int> SectionIds { get; set; } = new List<int>();
}