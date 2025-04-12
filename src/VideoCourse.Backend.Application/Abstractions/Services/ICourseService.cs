using VideoCourse.Backend.Application.Features.Courses.DTOs;
using VideoCourse.Backend.Shared.Utils.Requests;
using VideoCourse.Backend.Shared.Utils.Responses;
using VideoCourse.Backend.Shared.Utils.Results.Abstract;

public interface ICourseService
{
    Task<IResult> CreateAsync(CourseCreateDto dto);
    Task<IResult> UpdateAsync(CourseUpdateDto dto);
    Task<IDataResult<CourseDto>> GetByIdAsync(int id);
    Task<IDataResult<PaginatedResponse<CourseDto>>> GetVideoSectionsAsync(PageRequest pageRequest);
    Task<IResult> DeleteAsync(int id);
    Task<IResult> DeleteSectionsFromCourse(DeleteSectionsFromCourseDto dto);
    Task<IResult> AddSectionsToCourse(AddSectionsToCoursDto dto);
}