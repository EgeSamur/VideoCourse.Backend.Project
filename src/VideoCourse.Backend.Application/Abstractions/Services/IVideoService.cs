using VideoCourse.Backend.Application.Features.Videos.DTOs;
using VideoCourse.Backend.Shared.Utils.Requests;
using VideoCourse.Backend.Shared.Utils.Responses;
using VideoCourse.Backend.Shared.Utils.Results.Abstract;

public interface IVideoService
{
    Task<IResult> CreateAsync(VideoCreateDto dto);
    Task<IResult> UpdateAsync(VideoUpdateDto dto);
    Task<IDataResult<VideoDto>> GetByIdAsync(int id);
    Task<IDataResult<PaginatedResponse<VideoDto>>> GetVideosAsync(PageRequest pageRequest);
    Task<IResult> DeleteAsync(int id);
}
