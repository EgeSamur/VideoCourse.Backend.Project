using AutoMapper;
using VideoCourse.Backend.Application.Abstractions.Repositories;
using VideoCourse.Backend.Application.Common.Helpers;
using VideoCourse.Backend.Application.Features.Users.DTOs;
using VideoCourse.Backend.Application.Features.Videos.DTOs;
using VideoCourse.Backend.Domain.Entities;
using VideoCourse.Backend.Shared.CrossCuttingConcerns.Exceptions.Types;
using VideoCourse.Backend.Shared.Utils.Requests;
using VideoCourse.Backend.Shared.Utils.Responses;
using VideoCourse.Backend.Shared.Utils.Results.Abstract;
using VideoCourse.Backend.Shared.Utils.Results.Concrete;

namespace VideoCourse.Backend.Application.Features.Videos;

public class VideoService  : IVideoService
{
    // video controllerde S3simulate
    // ile upload similasyonu yapacağız unutma
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public VideoService(IMapper mapper, IUnitOfWork unitOfWork)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<IResult> CreateAsync(VideoCreateDto dto)
    {
        var video = new Video()
        {
            Title = dto.Title,
            Description = dto.Description,
            ThumbnailUrl = dto.ThumbnailUrl,
            DurationInSeconds = dto.DurationInSeconds,
            VideoUrl = dto.VideoUrl
        };
        await _unitOfWork.VideoRepository.AddAsync(video);
        await _unitOfWork.SaveChangesAsync();
        return new SuccessResult(MessageHelper.Created("Video"));
    }
    public async Task<IResult> DeleteAsync(int id)
    {
        // burada normalde s3ten de silmek gerek ? 
        var data = await _unitOfWork.VideoRepository.GetAsync(i => i.Id == id);
        if (data == null)
            throw new NotFoundException(MessageHelper.NotFound("Video"));
        await _unitOfWork.VideoRepository.HardDeleteAsync(data);
        await _unitOfWork.SaveChangesAsync();
        return new SuccessResult(MessageHelper.Deleted("Video"));
    }
    public async Task<IDataResult<VideoDto>> GetByIdAsync(int id)
    {
        var data = await _unitOfWork.VideoRepository.GetWithProjectionAsync(predicate: i => i.Id == id, selector: x => new VideoDto()
        {
            Id = x.Id,
            Title = x.Title,
            Description = x.Description,
            ThumbnailUrl = x.ThumbnailUrl,
            DurationInSeconds = x.DurationInSeconds,
            VideoUrl = x.VideoUrl
        });
        if (data == null)
            throw new NotFoundException(MessageHelper.NotFound("Video"));

        return new SuccessDataResult<VideoDto>(data, MessageHelper.FetchedById("Video"));
    }
    public async Task<IDataResult<PaginatedResponse<VideoDto>>> GetVideosAsync(PageRequest pageRequest)
    {
        var videos = await _unitOfWork.VideoRepository.GetListWithProjectionAsync(
           selector: x => new VideoDto()
           {
               Id = x.Id,
               Title = x.Title,
               Description = x.Description,
               ThumbnailUrl = x.ThumbnailUrl,
               DurationInSeconds = x.DurationInSeconds,
               VideoUrl = x.VideoUrl
           }
           ,
           orderBy: q => q.OrderBy(x => x.Id),
           size: pageRequest.Size,
           index: pageRequest.Index,
           isAll: pageRequest.IsAll);
        var result = _mapper.Map<PaginatedResponse<VideoDto>>(videos);
        return new SuccessDataResult<PaginatedResponse<VideoDto>>(result, MessageHelper.Listed("Videos"));
    }
    public async Task<IResult> UpdateAsync(VideoUpdateDto dto)
    {
        var data = await _unitOfWork.VideoRepository.GetAsync(predicate:  x => x.Id == dto.Id, enableTracking:true);
        if (data == null)
            throw new NotFoundException(MessageHelper.NotFound("Video"));
        data.Title = dto.Title;
        data.Description = dto.Description;
        data.ThumbnailUrl = dto.ThumbnailUrl;
        data.DurationInSeconds = dto.DurationInSeconds;
        data.VideoUrl = dto.VideoUrl;

        await _unitOfWork.SaveChangesAsync();
        return new SuccessResult(MessageHelper.Updated("Video"));

    }
}
