using AutoMapper;
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
        try
        {
            // Validate all video IDs with a single query
            if (dto.VideoIds != null && dto.VideoIds.Any())
            {
                var existingVideoIds = await _unitOfWork.VideoRepository.GetListWithProjectionAsync(
                    selector: v => v.Id,
                    predicate: v => dto.VideoIds.Contains(v.Id));

                // Convert to list to ensure we can use Count property and other operations
                var existingIdsList = existingVideoIds.Items.ToList();

                if (existingIdsList.Count != dto.VideoIds.Count)
                {
                    var missingIds = dto.VideoIds.Except(existingIdsList).ToList();
                    throw new NotFoundException($"Video(s) with ID(s) {string.Join(", ", missingIds)} not found.");
                }
            }

            // Create the course section
            var section = new CourseSection
            {
                Title = dto.Title,
                Description = dto.Description,
                OrderIndex = dto.OrderIndex
            };

            await _unitOfWork.CourseSectionRepository.AddAsync(section);
            await _unitOfWork.SaveChangesAsync();

            // Now that we have the section ID, add the video associations in bulk
            if (dto.VideoIds != null && dto.VideoIds.Any())
            {
                // Create all section video objects at once
                var sectionVideos = new List<SectionVideo>();

                for (int i = 0; i < dto.VideoIds.Count; i++)
                {
                    sectionVideos.Add(new SectionVideo
                    {
                        CourseSectionId = section.Id,
                        VideoId = dto.VideoIds[i],
                        OrderIndex = i
                    });
                }

                // Add all video associations in bulk
                foreach (var sectionVideo in sectionVideos)
                {
                    await _unitOfWork.SectionVideoRepository.AddAsync(sectionVideo);
                }

                // Save all video associations at once
                await _unitOfWork.SaveChangesAsync();
                
            }
            return new SuccessResult(MessageHelper.Created("Course Section"));
        }
        catch (Exception)
        {
            _unitOfWork.Rollback();
            throw;
        }
    }
    public async Task<IResult> UpdateAsync(CourseSectionUpdateDto dto)
    {
        try
        {
            var section = await _unitOfWork.CourseSectionRepository.GetAsync(s => s.Id == dto.Id, enableTracking: true);
            if (section == null)
                throw new NotFoundException(MessageHelper.NotFound("Course Section"));

            // Validate all video IDs with a single query
            if (dto.VideoIds != null && dto.VideoIds.Any())
            {
                var existingVideoIds = await _unitOfWork.VideoRepository.GetListWithProjectionAsync(
                    selector: v => v.Id,
                    predicate: v => dto.VideoIds.Contains(v.Id));

                var existingIdsList = existingVideoIds.Items.ToList();

                if (existingIdsList.Count != dto.VideoIds.Count)
                {
                    var missingIds = dto.VideoIds.Except(existingIdsList).ToList();
                    throw new NotFoundException($"Video(s) with ID(s) {string.Join(", ", missingIds)} not found.");
                }
            }

            // Update section properties
            section.Title = dto.Title;
            section.Description = dto.Description;
            section.OrderIndex = dto.OrderIndex;
            await _unitOfWork.SaveChangesAsync();

            // Update video associations - get all current video associations in one query
            var currentSectionVideos = await _unitOfWork.SectionVideoRepository.GetListAsync(
                sv => sv.CourseSectionId == section.Id);

            // Delete all existing video associations if we're changing them
            if (dto.VideoIds != null)
            {
                // Get the current video IDs for comparison
                var currentVideoIds = currentSectionVideos.Items.Select(sv => sv.VideoId).ToList();

                // Only proceed with video changes if there's actually a difference
                if (!dto.VideoIds.OrderBy(id => id).SequenceEqual(currentVideoIds.OrderBy(id => id)))
                {
                    // Remove all existing video associations
                    foreach (var sectionVideo in currentSectionVideos.Items)
                    {
                        await _unitOfWork.SectionVideoRepository.HardDeleteAsync(sectionVideo);
                    }
                    await _unitOfWork.SaveChangesAsync();

                    // Add new video associations
                    if (dto.VideoIds.Any())
                    {
                        // Create all section video objects at once
                        var newSectionVideos = new List<SectionVideo>();

                        for (int i = 0; i < dto.VideoIds.Count; i++)
                        {
                            newSectionVideos.Add(new SectionVideo
                            {
                                CourseSectionId = section.Id,
                                VideoId = dto.VideoIds[i],
                                OrderIndex = i
                            });
                        }

                        // Add all video associations in bulk
                        foreach (var newSectionVideo in newSectionVideos)
                        {
                            await _unitOfWork.SectionVideoRepository.AddAsync(newSectionVideo);
                        }

                        // Save all changes at once
                        await _unitOfWork.SaveChangesAsync();
                    }
                }
            }

            return new SuccessResult(MessageHelper.Updated("Course Section"));
        }
        catch (Exception)
        {
            _unitOfWork.Rollback();
            throw;
        }
    }
    public async Task<IResult> DeleteAsync(int id)
    {
        var courseSection = await _unitOfWork.CourseSectionRepository.GetAsync(i => i.Id == id);
        if (courseSection == null) 
            throw new NotFoundException(MessageHelper.NotFound("VideoSection"));
        await _unitOfWork.CourseSectionRepository.DeleteAsync(courseSection);
        await _unitOfWork.SaveChangesAsync();
        return new SuccessResult(MessageHelper.Deleted("Course Section"));

    }
    public async Task<IDataResult<CourseSectionDto>> GetByIdAsync(int id)
    {
        var data = await _unitOfWork.CourseSectionRepository.GetWithProjectionAsync(
            predicate: i => i.Id == id,
            selector: x => new CourseSectionDto()
            {
                CourseId = x.CourseId,
                Title = x.Title,
                Description = x.Description,
                OrderIndex = x.OrderIndex,
                Videos = x.SectionVideos
                .OrderBy(sv => sv.Id)
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
                CourseId = i.CourseId,
                Title = i.Title,
                Description = i.Description,
                OrderIndex = i.OrderIndex,
                Videos = i.SectionVideos
                .OrderBy(sv => sv.Id)
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
        // Bölümün var olup olmadığını kontrol et
        var section = await _unitOfWork.CourseSectionRepository.GetAsync(s => s.Id == dto.CourseSectionId);
        if (section == null)
            return new ErrorResult(MessageHelper.NotFound("Course Section"));

        // Tüm video ID'lerini tek sorguda doğrula
        if (dto.VideoIds == null || !dto.VideoIds.Any())
            return new ErrorResult("No video IDs provided.");

        var existingVideoIds = await _unitOfWork.VideoRepository.GetListWithProjectionAsync(
            selector: v => v.Id,
            predicate: v => dto.VideoIds.Contains(v.Id));

        var existingIdsList = existingVideoIds.Items.ToList();

        if (existingIdsList.Count != dto.VideoIds.Count)
        {
            var missingIds = dto.VideoIds.Except(existingIdsList).ToList();
            return new ErrorResult($"Video(s) with ID(s) {string.Join(", ", missingIds)} not found.");
        }

        // Bölümdeki mevcut videoları çek
        var existingSectionVideos = await _unitOfWork.SectionVideoRepository.GetListAsync(
            sv => sv.CourseSectionId == dto.CourseSectionId);

        // Mevcut video ID'lerini al
        var existingSectionVideoIds = existingSectionVideos.Items.Select(sv => sv.VideoId).ToList();

        // Sadece yeni eklenecek videoları belirle (duplikasyonu önle)
        var newVideoIds = dto.VideoIds.Except(existingSectionVideoIds).ToList();

        if (!newVideoIds.Any())
            return new SuccessResult("All videos are already associated with this section.");

        // Yeni eklenecek videolar için başlangıç sıra numarasını belirle
        int startOrderIndex = 0;

        if (existingSectionVideos.Items.Any())
            startOrderIndex = existingSectionVideos.Items.Max(sv => sv.OrderIndex) + 1;

        // Yeni videoları bölüme ekle
        var newSectionVideos = new List<SectionVideo>();
        int orderIndex = startOrderIndex;

        foreach (var videoId in newVideoIds)
        {
            newSectionVideos.Add(new SectionVideo
            {
                CourseSectionId = dto.CourseSectionId,
                VideoId = videoId,
                OrderIndex = orderIndex++
            });
        }

        // Toplu olarak ekle
        foreach (var sectionVideo in newSectionVideos)
        {
            await _unitOfWork.SectionVideoRepository.AddAsync(sectionVideo);
        }

        await _unitOfWork.SaveChangesAsync();

        return new SuccessResult($"{newVideoIds.Count} video(s) added to the course section.");
    }
    public async Task<IResult> DeleteVideoFromCourseSectionAsync(DeleteVideosFromCourseSectionDto dto)
    {
        // Bölümün var olup olmadığını kontrol et
        var section = await _unitOfWork.CourseSectionRepository.GetAsync(s => s.Id == dto.CourseSectionId);
        if (section == null)
            return new ErrorResult(MessageHelper.NotFound("Course Section"));

        if (dto.VideoIds == null || !dto.VideoIds.Any())
            return new ErrorResult("No video IDs provided.");

        // Bölüm-video ilişkilerini kontrol et
        var sectionVideos = await _unitOfWork.SectionVideoRepository.GetListAsync(
            sv => sv.CourseSectionId == dto.CourseSectionId && dto.VideoIds.Contains(sv.VideoId));

        if (!sectionVideos.Items.Any())
            return new ErrorResult("None of the provided video IDs are associated with this section.");

        var foundVideoIds = sectionVideos.Items.Select(sv => sv.VideoId).ToList();
        var notFoundVideoIds = dto.VideoIds.Except(foundVideoIds).ToList();

        if (notFoundVideoIds.Any())
        {
            return new SuccessResult($"Warning: Video(s) with ID(s) {string.Join(", ", notFoundVideoIds)} were not found in this section.");
        }

        // İlişkileri sil
        foreach (var sectionVideo in sectionVideos.Items)
        {
            await _unitOfWork.SectionVideoRepository.HardDeleteAsync(sectionVideo);
        }
        await _unitOfWork.SaveChangesAsync();

        // Kalan videoların sırasını güncelle
        var remainingVideos = await _unitOfWork.SectionVideoRepository.GetListAsync(
            sv => sv.CourseSectionId == dto.CourseSectionId,
            orderBy: q => q.OrderBy(sv => sv.OrderIndex));

        if (remainingVideos.Items.Any())
        {
            int orderIndex = 0;
            foreach (var video in remainingVideos.Items)
            {
                video.OrderIndex = orderIndex++;
            }
            await _unitOfWork.SaveChangesAsync();
        }

        return new SuccessResult($"{sectionVideos.Items.Count} video(s) removed from section.");
    }
}


// sectiondan tane ile video silme tane ile video ekleme 
// courseden tane ile section çıkartma tane ile video ekleme 
// section ve courseler hard delete yapılamaz soft delete
// video sıralaması için frontEnd önemli sıra değiştirmek için ne yapacağımızı düşüneceğiz.