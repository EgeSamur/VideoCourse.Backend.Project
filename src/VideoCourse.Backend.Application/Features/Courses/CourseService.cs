using AutoMapper;
using Microsoft.EntityFrameworkCore;
using VideoCourse.Backend.Application.Abstractions.Repositories;
using VideoCourse.Backend.Application.Common.Helpers;
using VideoCourse.Backend.Application.Features.Courses.DTOs;
using VideoCourse.Backend.Application.Features.CourseSections.DTOs;
using VideoCourse.Backend.Application.Features.Videos.DTOs;
using VideoCourse.Backend.Domain.Entities;
using VideoCourse.Backend.Shared.CrossCuttingConcerns.Exceptions.Types;
using VideoCourse.Backend.Shared.Utils.Requests;
using VideoCourse.Backend.Shared.Utils.Responses;
using VideoCourse.Backend.Shared.Utils.Results.Abstract;
using VideoCourse.Backend.Shared.Utils.Results.Concrete;

namespace VideoCourse.Backend.Application.Features.Courses;

public class CourseService : ICourseService
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public CourseService(IMapper mapper, IUnitOfWork unitOfWork)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<IResult> CreateAsync(CourseCreateDto dto)
    {
        var course = new Course()
        {
            Title = dto.Title,
            Description = dto.Description,
            ThumbnailUrl = dto.ThumbnailUrl,
            IsActive = true
        };

        await _unitOfWork.CourseRepository.AddAsync(course);
        await _unitOfWork.SaveChangesAsync();

        return new SuccessResult(MessageHelper.Created("Course"));
    }
    public async Task<IResult> UpdateAsync(CourseUpdateDto dto)
    {
        var data = await _unitOfWork.CourseRepository.GetAsync(
            predicate: i => i.Id == dto.Id, enableTracking: true);
        if (data == null)
            throw new NotFoundException(MessageHelper.NotFound("Course"));

        data.Title = dto.Title;
        data.Description = dto.Description;
        data.ThumbnailUrl = dto.ThumbnailUrl;
        data.IsActive = dto.IsActive;

        await _unitOfWork.SaveChangesAsync();
        return new SuccessResult(MessageHelper.Updated("Course"));
    }
    public async Task<IResult> DeleteAsync(int id)
    {
        var data = await _unitOfWork.CourseRepository.GetAsync(
            predicate: i => i.Id == id, enableTracking: true);
        if (data == null)
            throw new NotFoundException(MessageHelper.NotFound("Course"));

        await _unitOfWork.CourseRepository.DeleteAsync(data);
        await _unitOfWork.SaveChangesAsync();

        return new SuccessResult(MessageHelper.Deleted("Course"));
    }
    public async Task<IResult> AddSectionsToCourse(AddSectionsToCoursDto dto)
    {
        // courseSection idleri checkbox şeklinde olmalı knkaaa mesela 1 2, var 3 4 5 eklencek 1,2,3,4,5 gelmeli.
        var course = await _unitOfWork.CourseRepository.GetAsync(i => i.Id == dto.CourseId,
            include: x => x.Include(i => i.CourseCourseSections)
            .ThenInclude(i => i.Section),
            enableTracking: true);
        if (course == null)
            return new ErrorResult(MessageHelper.NotFound("Course"));
        course.CourseCourseSections.Clear();
        var order = 0;
        foreach (var courseSectionId in dto.SectionIds)
        {
            course.CourseCourseSections.Add(new CourseCourseSection()
            {
                CourseId = course.Id,
                CourseSectionId = courseSectionId,
                OrderIndex = order
            });
            order++;
        }
        await _unitOfWork.SaveChangesAsync();
        return new SuccessResult(MessageHelper.Updated("Course Course Sections"));
    }
    public async Task<IDataResult<CourseDto>> GetByIdAsync(int id)
    {

        var data = await _unitOfWork.CourseRepository.GetWithProjectionAsync(
            predicate: i => i.Id == id,
            selector: x => new CourseDto()
            {
                Id = x.Id,
                Title = x.Title,
                Description = x.Description,
                ThumbnailUrl = x.ThumbnailUrl,
                IsActive = x.IsActive,
                Sections = x.CourseCourseSections
                    .OrderBy(cs => cs.OrderIndex)
                    .Select(cs => new CourseSectionDto()
                    {
                        CourseSectionId = cs.CourseSectionId,
                        Title = cs.Section.Title,
                        Description = cs.Section.Description,
                        Videos = cs.Section.CourseSectionVideos
                            .OrderBy(cv => cv.OrderIndex)
                            .Select(cv => new VideoDto()
                            {
                                Id = cv.Video.Id,
                                Title = cv.Video.Title,
                                Description = cv.Video.Description,
                                ThumbnailUrl = cv.Video.ThumbnailUrl,
                                VideoUrl = cv.Video.VideoUrl,
                                DurationInSeconds = cv.Video.DurationInSeconds
                            }).ToList()
                    }).ToList()
            });

        if (data == null)
            throw new NotFoundException(MessageHelper.NotFound("Course"));

        return new SuccessDataResult<CourseDto>(data, MessageHelper.FetchedById("Course"));


    }
    public async Task<IDataResult<PaginatedResponse<CourseDto>>> GetCoursesAsync(PageRequest pageRequest)
    {
        var courses = await _unitOfWork.CourseRepository.GetListWithProjectionAsync(
           selector: i => new CourseDto()
           {
               Id = i.Id,
               Title = i.Title,
               Description = i.Description,
               ThumbnailUrl = i.ThumbnailUrl,
               IsActive = i.IsActive,
               Sections = i.CourseCourseSections
                   .OrderBy(cs => cs.OrderIndex)
                   .Select(cs => new CourseSectionDto()
                   {
                       CourseSectionId = cs.Section.Id,
                       Title = cs.Section.Title,
                       Description = cs.Section.Description,
                       Videos = cs.Section.CourseSectionVideos
                           .OrderBy(cv => cv.OrderIndex)
                           .Select(cv => new VideoDto()
                           {
                               Id = cv.Video.Id,
                               Title = cv.Video.Title,
                               Description = cv.Video.Description,
                               ThumbnailUrl = cv.Video.ThumbnailUrl,
                               VideoUrl = cv.Video.VideoUrl,
                               DurationInSeconds = cv.Video.DurationInSeconds
                           }).ToList()
                   }).ToList()
           },
           size: pageRequest.Size,
           index: pageRequest.Index,
           isAll: pageRequest.IsAll);

        var result = _mapper.Map<PaginatedResponse<CourseDto>>(courses);
        return new SuccessDataResult<PaginatedResponse<CourseDto>>(result, MessageHelper.Listed("Courses"));

    }
    public async Task<IResult> DeleteSectionsFromCourse(DeleteSectionsFromCourseDto dto)
    {
        var course = await _unitOfWork.CourseRepository.GetAsync(
             predicate: s => s.Id == dto.CourseId,
             include: x => x.Include(s => s.CourseCourseSections).ThenInclude(i=>i.Section),
             enableTracking: true);

        if (course == null)
            return new ErrorResult(MessageHelper.NotFound("Course"));

        if (dto.SectionIds == null || !dto.SectionIds.Any())
            return new ErrorResult("No course section IDs provided.");

        var initialCount = course.CourseCourseSections.Count;

        // Sadece eşleşen videoId’leri ilişkiden çıkar
        course.CourseCourseSections = course.CourseCourseSections
            .Where(sv => !dto.SectionIds.Contains(sv.CourseSectionId))
            .OrderBy(sv => sv.OrderIndex) // Sıralama bozulmasın
            .ToList();

        // OrderIndex'leri yeniden sırala
        int order = 0;
        foreach (var courseSection in course.CourseCourseSections)
            courseSection.OrderIndex = order++;

        await _unitOfWork.SaveChangesAsync();

        var deletedCount = initialCount - course.CourseCourseSections.Count;
        return new SuccessResult($"{deletedCount} course section(s) removed from section.");
    }
    public async Task<IDataResult<CourseDto>> SwapCourseSection(SwapCourseSectionDto dto)
    {
        var course = await _unitOfWork.CourseRepository.GetAsync(
            predicate: s => s.Id == dto.CourseId,
            include: x => x.Include(s => s.CourseCourseSections)
                           .ThenInclude(v => v.Section),
            enableTracking: true);

        if (course == null)
            return new ErrorDataResult<CourseDto>(MessageHelper.NotFound("Course"));

        var firstCourseSection = course.CourseCourseSections.FirstOrDefault(v => v.CourseSectionId == dto.FirstCourseSectionId);
        var secondCourseSection = course.CourseCourseSections.FirstOrDefault(v => v.CourseSectionId == dto.SecondCourseSectionId);

        if (firstCourseSection == null || secondCourseSection == null)
            return new ErrorDataResult<CourseDto>(MessageHelper.NotFound("One or both course sections"));

        // OrderIndex swap
        (firstCourseSection.OrderIndex, secondCourseSection.OrderIndex) = (secondCourseSection.OrderIndex, firstCourseSection.OrderIndex);

        await _unitOfWork.SaveChangesAsync();

        // Güncellenmiş listeyi dön
        var updatedDto = new CourseDto
        {
            Title = course.Title,
            Description = course.Description,
            Sections = course.CourseCourseSections
                .OrderBy(ccv => ccv.OrderIndex) // burada ID yerine sıralama esas alınmalı
                .Select(ccv => new CourseSectionDto
                {
                    CourseSectionId = ccv.Section.Id,
                    Title = ccv.Section.Title,
                    Description = ccv.Section.Description,
                    Videos = ccv.Section.CourseSectionVideos.OrderBy(csv => csv.OrderIndex).Select(csv => new VideoDto()
                    {
                        Id = csv.Video.Id,
                        DurationInSeconds = csv.Video.DurationInSeconds,
                        Description = csv.Video.Description,
                        ThumbnailUrl = csv.Video.ThumbnailUrl,
                        Title = csv.Video.Title,
                        VideoUrl = csv.Video.VideoUrl
                    }).ToList()
                }).ToList()
        };

        return new SuccessDataResult<CourseDto>(updatedDto, MessageHelper.Updated("Course Section Order"));
    }
}
