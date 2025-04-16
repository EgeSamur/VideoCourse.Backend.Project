using AutoMapper;
using DocumentFormat.OpenXml.Office.CustomUI;
using Microsoft.EntityFrameworkCore;
using VideoCourse.Backend.Application.Abstractions.Repositories;
using VideoCourse.Backend.Application.Common.Helpers;
using VideoCourse.Backend.Application.Features.CourseSections.DTOs;
using VideoCourse.Backend.Application.Features.Videos.DTOs;
using VideoCourse.Backend.Domain.Entities;
using VideoCourse.Backend.Shared.CrossCuttingConcerns.Exceptions.Types;
using VideoCourse.Backend.Shared.Utils.Requests;
using VideoCourse.Backend.Shared.Utils.Responses;
using VideoCourse.Backend.Shared.Utils.Results.Abstract;
using VideoCourse.Backend.Shared.Utils.Results.Concrete;

namespace VideoCourse.Backend.Application.Features.CourseSections;

public class CourseSectionService : ICourseSectionService
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public CourseSectionService(IMapper mapper, IUnitOfWork unitOfWork)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }
    public async Task<IResult> CreateAsync(CourseSectionCreateDto dto)
    {
        var section = new CourseSection()
        {
            Title = dto.Title,
            Description = dto.Description
        };
        await _unitOfWork.CourseSectionRepository.AddAsync(section);
        await _unitOfWork.SaveChangesAsync();
        return new SuccessResult(MessageHelper.Created("Course Section"));
    }
    public async Task<IResult> UpdateAsync(CourseSectionUpdateDto dto)
    {
        var data = await _unitOfWork.CourseSectionRepository.GetAsync(predicate: i => i.Id == dto.Id, enableTracking: true);
        if (data == null)
            throw new NotFoundException(MessageHelper.NotFound("Course Section"));
        data.Title = dto.Title;
        data.Description = dto.Description;
        await _unitOfWork.SaveChangesAsync();
        return new SuccessResult(MessageHelper.Updated("Course Section"));
    }
    public async Task<IResult> DeleteAsync(int id)
    {
        var data = await _unitOfWork.CourseSectionRepository.GetAsync(predicate: i => i.Id == id, enableTracking: true);
        if (data == null)
            throw new NotFoundException(MessageHelper.NotFound("Course Section"));
        await _unitOfWork.CourseSectionRepository.DeleteAsync(data);
        await _unitOfWork.SaveChangesAsync();
        return new SuccessResult(MessageHelper.Deleted("Course Section"));
    }
    public async Task<IDataResult<CourseSectionDto>> GetByIdAsync(int id)
    {
        var data = await _unitOfWork.CourseSectionRepository.GetWithProjectionAsync(
            predicate: i => i.Id == id,
            selector: x => new CourseSectionDto()
            {
                CourseSectionId = x.Id,
                Title = x.Title,
                Description = x.Description,
                Videos = x.CourseSectionVideos
                .OrderBy(sv => sv.OrderIndex)
                .Select(sv => new VideoDto()
                {
                    Id = sv.Video.Id,
                    Title = sv.Video.Title,
                    Description = sv.Video.Description,
                    ThumbnailUrl = sv.Video.ThumbnailUrl,
                    VideoUrl = sv.Video.VideoUrl,
                    DurationInSeconds = sv.Video.DurationInSeconds
                }).ToList()

            });
        if (data == null)
            throw new NotFoundException(MessageHelper.NotFound("Course Section"));
        return new SuccessDataResult<CourseSectionDto>(data, MessageHelper.FetchedById("Course Section"));

    }
    public async Task<IDataResult<PaginatedResponse<CourseSectionDto>>> GetVideoSectionsAsync(PageRequest pageRequest)
    {
        var videoSections = await _unitOfWork.CourseSectionRepository.GetListWithProjectionAsync(
            selector: i => new CourseSectionDto()
            {
                CourseSectionId = i.Id,
                Title = i.Title,
                Description = i.Description,
                Videos = i.CourseSectionVideos
                .OrderBy(sv => sv.OrderIndex)
                .Select(sv => new VideoDto()
                {
                    Id = sv.Video.Id,
                    Title = sv.Video.Title,
                    Description = sv.Video.Description,
                    ThumbnailUrl = sv.Video.ThumbnailUrl,
                    VideoUrl = sv.Video.VideoUrl,
                    DurationInSeconds = sv.Video.DurationInSeconds
                }).ToList()
            });
        var result = _mapper.Map<PaginatedResponse<CourseSectionDto>>(videoSections);
        return new SuccessDataResult<PaginatedResponse<CourseSectionDto>>(result, MessageHelper.Listed("Video Sections"));
    }
    public async Task<IResult> AddVideosToCourseSection(AddVideosCourseSectionDto dto)
    {
        // video idleri checkbox şeklinde olmalı knkaaa mesela 1 2, var 3 4 5 eklencek 1,2,3,4,5 gelmeli.
        var courseSection = await _unitOfWork.CourseSectionRepository.GetAsync(predicate: i => i.Id == dto.CourseSectionId,
            include: x => x.Include(i => i.CourseSectionVideos)
            .ThenInclude(i => i.Video), enableTracking: true);
        if (courseSection == null)
            return new ErrorResult(MessageHelper.NotFound("Course Section"));
        courseSection.CourseSectionVideos.Clear();
        var order = 0;
        foreach (var videoId in dto.VideoIds)
        {
            courseSection.CourseSectionVideos.Add(new CourseSectionVideo()
            {
                CourseSectionId = courseSection.Id,
                VideoId = videoId,
                OrderIndex = order
            });
            order++;
        }
        await _unitOfWork.SaveChangesAsync();

     
        return new SuccessResult(MessageHelper.Updated("Video Section Videos"));
    }
    public async Task<IResult> DeleteVideoFromCourseSectionAsync(DeleteVideosFromCourseSectionDto dto)
    {
        var section = await _unitOfWork.CourseSectionRepository.GetAsync(
            predicate: s => s.Id == dto.CourseSectionId,
            include: x => x.Include(s => s.CourseSectionVideos),
            enableTracking: true);

        if (section == null)
            return new ErrorResult(MessageHelper.NotFound("Course Section"));

        if (dto.VideoIds == null || !dto.VideoIds.Any())
            return new ErrorResult("No video IDs provided.");

        var initialCount = section.CourseSectionVideos.Count;

        // Sadece eşleşen videoId’leri ilişkiden çıkar
        section.CourseSectionVideos = section.CourseSectionVideos
            .Where(sv => !dto.VideoIds.Contains(sv.VideoId))
            .OrderBy(sv => sv.OrderIndex) // Sıralama bozulmasın
            .ToList();

        // OrderIndex'leri yeniden sırala
        int order = 0;
        foreach (var video in section.CourseSectionVideos)
            video.OrderIndex = order++;

        await _unitOfWork.SaveChangesAsync();

        var deletedCount = initialCount - section.CourseSectionVideos.Count;
        return new SuccessResult($"{deletedCount} video(s) removed from section.");
    }
    public async Task<IDataResult<CourseSectionDto>> SwapSectionVideos(SwapSectionVideosDto dto)
    {
        var section = await _unitOfWork.CourseSectionRepository.GetAsync(
            predicate: s => s.Id == dto.CourseSectionId,
            include: x => x.Include(s => s.CourseSectionVideos)
                           .ThenInclude(v => v.Video),
            enableTracking: true);

        if (section == null)
            return new ErrorDataResult<CourseSectionDto>(MessageHelper.NotFound("Course Section"));

        var firstVideo = section.CourseSectionVideos.FirstOrDefault(v => v.VideoId == dto.FirstVideoId);
        var secondVideo = section.CourseSectionVideos.FirstOrDefault(v => v.VideoId == dto.SecondVideoId);

        if (firstVideo == null || secondVideo == null)
            return new ErrorDataResult<CourseSectionDto>(MessageHelper.NotFound("One or both videos"));

        // OrderIndex swap
        (firstVideo.OrderIndex, secondVideo.OrderIndex) = (secondVideo.OrderIndex, firstVideo.OrderIndex);

        await _unitOfWork.SaveChangesAsync();

        // Güncellenmiş listeyi dön
        var updatedDto = new CourseSectionDto
        {
            Title = section.Title,
            Description = section.Description,
            Videos = section.CourseSectionVideos
                .OrderBy(v => v.OrderIndex) // burada ID yerine sıralama esas alınmalı
                .Select(v => new VideoDto
                {
                    Id = v.Video.Id,
                    Title = v.Video.Title,
                    Description = v.Video.Description,
                    ThumbnailUrl = v.Video.ThumbnailUrl,
                    VideoUrl = v.Video.VideoUrl,
                    DurationInSeconds = v.Video.DurationInSeconds
                }).ToList()
        };

        return new SuccessDataResult<CourseSectionDto>(updatedDto, MessageHelper.Updated("Section Videos"));
    }

}

// önce section oluşturulur sonra sectionlara video eklenir. videolar için ekle ve update et oluşturacağız
// section ve courseler hard delete yapılamaz soft delete
