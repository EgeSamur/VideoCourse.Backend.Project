using VideoCourse.Backend.Application.Features.CourseSections.DTOs;
using VideoCourse.Backend.Shared.Utils.Requests;
using VideoCourse.Backend.Shared.Utils.Responses;
using VideoCourse.Backend.Shared.Utils.Results.Abstract;
// şimdi course section -> burada addvideo to section var ve bunun updateside olmalı
public interface ICourseSectionService
{
    Task<IResult> CreateAsync(CourseSectionCreateDto dto);
    Task<IResult> UpdateAsync(CourseSectionUpdateDto dto);
    Task<IResult> AddVideosToCourseSection(AddVideosCourseSectionDto dto);
    Task<IDataResult<CourseSectionDto>> GetByIdAsync(int id);
    Task<IDataResult<PaginatedResponse<CourseSectionDto>>> GetVideoSectionsAsync(PageRequest pageRequest);
    Task<IResult> DeleteVideoFromCourseSectionAsync(DeleteVideosFromCourseSectionDto dto); 
    Task<IResult> DeleteAsync(int id);
}
