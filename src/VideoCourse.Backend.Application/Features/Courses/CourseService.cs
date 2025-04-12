using AutoMapper;
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
        try
        {
            // Validate section IDs with a single query
            if (dto.SectionIds != null && dto.SectionIds.Any())
            {
                var existingSectionIds = await _unitOfWork.CourseSectionRepository.GetListWithProjectionAsync(
                    selector: s => s.Id,
                    predicate: s => dto.SectionIds.Contains(s.Id));

                // Convert to list to ensure we can use Count property and other operations
                var existingIdsList = existingSectionIds.Items.ToList();

                if (existingIdsList.Count != dto.SectionIds.Count)
                {
                    var missingIds = dto.SectionIds.Except(existingIdsList).ToList();
                    throw new NotFoundException($"Section(s) with ID(s) {string.Join(", ", missingIds)} not found.");
                }
            }

            // Create the course
            var course = new Course
            {
                Title = dto.Title,
                Description = dto.Description,
                ThumbnailUrl = dto.ThumbnailUrl,
                IsActive = dto.IsActive
            };

            await _unitOfWork.CourseRepository.AddAsync(course);
            await _unitOfWork.SaveChangesAsync();

            // Associate sections with course - first get all the sections in one query
            if (dto.SectionIds != null && dto.SectionIds.Any())
            {
                var existingSections = await _unitOfWork.CourseSectionRepository.GetListAsync(
                    predicate: s => dto.SectionIds.Contains(s.Id));

                int orderIndex = 0;

                // Process all sections
                foreach (var existingSection in existingSections.Items)
                {
                    // Create a new section for this course
                    var newSection = new CourseSection
                    {
                        CourseId = course.Id,
                        Title = existingSection.Title,
                        Description = existingSection.Description,
                        OrderIndex = orderIndex++
                    };

                    await _unitOfWork.CourseSectionRepository.AddAsync(newSection);
                    await _unitOfWork.SaveChangesAsync();

                    // Get all videos for this section in one query
                    var existingSectionVideos = await _unitOfWork.SectionVideoRepository.GetListAsync(
                        sv => sv.CourseSectionId == existingSection.Id,
                        orderBy: q => q.OrderBy(sv => sv.OrderIndex));

                    // Prepare all new section videos at once
                    var newSectionVideos = existingSectionVideos.Items.Select(esv => new SectionVideo
                    {
                        CourseSectionId = newSection.Id,
                        VideoId = esv.VideoId,
                        OrderIndex = esv.OrderIndex
                    }).ToList();

                    // Bulk add all videos
                    foreach (var newSectionVideo in newSectionVideos)
                    {
                        await _unitOfWork.SectionVideoRepository.AddAsync(newSectionVideo);
                    }
                }

                // Save all changes at once
                await _unitOfWork.SaveChangesAsync();
            }

            return new SuccessResult(MessageHelper.Created("Course"));
        }
        catch (Exception)
        {
            _unitOfWork.Rollback();
            throw;
        }
    }
    public async Task<IResult> UpdateAsync(CourseUpdateDto dto)
    {
        try
        {
            var course = await _unitOfWork.CourseRepository.GetAsync(c => c.Id == dto.Id, enableTracking: true);
            if (course == null)
                throw new NotFoundException(MessageHelper.NotFound("Course"));

            // Update course properties
            course.Title = dto.Title;
            course.Description = dto.Description;
            course.ThumbnailUrl = dto.ThumbnailUrl;
            course.IsActive = dto.IsActive;
            await _unitOfWork.SaveChangesAsync();

            // If we're updating sections, validate all section IDs at once
            if (dto.SectionIds != null && dto.SectionIds.Any())
            {
                var existingSectionIds = await _unitOfWork.CourseSectionRepository.GetListWithProjectionAsync(
                    selector: s => s.Id,
                    predicate: s => dto.SectionIds.Contains(s.Id));

                var existingIdsList = existingSectionIds.Items.ToList();

                if (existingIdsList.Count != dto.SectionIds.Count)
                {
                    var missingIds = dto.SectionIds.Except(existingIdsList).ToList();
                    throw new NotFoundException($"Section(s) with ID(s) {string.Join(", ", missingIds)} not found.");
                }

                // Get current sections in one query
                var currentSections = await _unitOfWork.CourseSectionRepository.GetListAsync(
                    s => s.CourseId == course.Id);

                // Get IDs of current sections
                var currentSectionIds = currentSections.Items.Select(s => s.Id).ToList();

                // Find sections to remove (in current but not in incoming list)
                var sectionsToRemove = currentSections.Items
                    .Where(s => !dto.SectionIds.Contains(s.Id))
                    .ToList();

                // Remove these sections and their videos
                if (sectionsToRemove.Any())
                {
                    // Get all section IDs to remove
                    var sectionIdsToRemove = sectionsToRemove.Select(s => s.Id).ToList();

                    // Get all videos for these sections in one query
                    var videosToRemove = await _unitOfWork.SectionVideoRepository.GetListAsync(
                        sv => sectionIdsToRemove.Contains(sv.CourseSectionId));

                    // Remove all videos
                    foreach (var video in videosToRemove.Items)
                    {
                        await _unitOfWork.SectionVideoRepository.HardDeleteAsync(video);
                    }

                    // Remove all sections
                    foreach (var section in sectionsToRemove)
                    {
                        await _unitOfWork.CourseSectionRepository.HardDeleteAsync(section);
                    }

                    await _unitOfWork.SaveChangesAsync();
                }

                // Add new sections (in incoming list but not in current)
                var sectionIdsToAdd = dto.SectionIds
                    .Except(currentSectionIds)
                    .ToList();

                if (sectionIdsToAdd.Any())
                {
                    // Get all sections to add in one query
                    var sectionsToAdd = await _unitOfWork.CourseSectionRepository.GetListAsync(
                        s => sectionIdsToAdd.Contains(s.Id));

                    int orderIndex = currentSections.Items.Count;

                    foreach (var sectionToAdd in sectionsToAdd.Items)
                    {
                        // Create a new section for this course
                        var newSection = new CourseSection
                        {
                            CourseId = course.Id,
                            Title = sectionToAdd.Title,
                            Description = sectionToAdd.Description,
                            OrderIndex = orderIndex++
                        };

                        await _unitOfWork.CourseSectionRepository.AddAsync(newSection);
                        await _unitOfWork.SaveChangesAsync();

                        // Get all videos for this section in one query
                        var sectionVideos = await _unitOfWork.SectionVideoRepository.GetListAsync(
                            sv => sv.CourseSectionId == sectionToAdd.Id,
                            orderBy: q => q.OrderBy(sv => sv.OrderIndex));

                        // Add all videos at once
                        var newSectionVideos = sectionVideos.Items.Select(sv => new SectionVideo
                        {
                            CourseSectionId = newSection.Id,
                            VideoId = sv.VideoId,
                            OrderIndex = sv.OrderIndex
                        }).ToList();

                        foreach (var newSectionVideo in newSectionVideos)
                        {
                            await _unitOfWork.SectionVideoRepository.AddAsync(newSectionVideo);
                        }
                    }

                    await _unitOfWork.SaveChangesAsync();
                }
            }

            return new SuccessResult(MessageHelper.Updated("Course"));
        }
        catch (Exception)
        {
            _unitOfWork.Rollback();
            throw;
        };
    }
    public async Task<IResult> DeleteAsync(int id)
    {
        try
        {
            var course = await _unitOfWork.CourseRepository.GetAsync(c => c.Id == id);
            if (course == null)
                throw new NotFoundException(MessageHelper.NotFound("Course"));

            // Get sections for this course
            var sections = await _unitOfWork.CourseSectionRepository.GetListAsync(s => s.CourseId == id);

            foreach (var section in sections.Items)
            {
                // Get section videos
                var sectionVideos = await _unitOfWork.SectionVideoRepository.GetListAsync(sv => sv.CourseSectionId == section.Id);

                // Delete section videos
                foreach (var sectionVideo in sectionVideos.Items)
                {
                    await _unitOfWork.SectionVideoRepository.HardDeleteAsync(sectionVideo);
                }

                // Delete section
                await _unitOfWork.CourseSectionRepository.HardDeleteAsync(section);
            }

            // Delete course
            await _unitOfWork.CourseRepository.DeleteAsync(course);
            await _unitOfWork.SaveChangesAsync();

            return new SuccessResult(MessageHelper.Deleted("Course"));
        }
        catch (Exception)
        {
            _unitOfWork.Rollback();
            throw;
        }
    }
    public async Task<IResult> AddSectionsToCourse(AddSectionsToCoursDto dto)
    {
        try
        {
            // Kursun var olup olmadığını kontrol et
            var course = await _unitOfWork.CourseRepository.GetAsync(c => c.Id == dto.CourseId);
            if (course == null)
                return new ErrorResult(MessageHelper.NotFound("Course"));

            // Tüm section ID'lerini tek sorguda doğrula
            if (dto.SectionIds == null || !dto.SectionIds.Any())
                return new ErrorResult("No section IDs provided.");

            var existingSectionIds = await _unitOfWork.CourseSectionRepository.GetListWithProjectionAsync(
                selector: s => s.Id,
                predicate: s => dto.SectionIds.Contains(s.Id));

            var existingIdsList = existingSectionIds.Items.ToList();

            if (existingIdsList.Count != dto.SectionIds.Count)
            {
                var missingIds = dto.SectionIds.Except(existingIdsList).ToList();
                return new ErrorResult($"Section(s) with ID(s) {string.Join(", ", missingIds)} not found.");
            }

            // Kurstaki mevcut bölümleri çek
            var existingCourseSections = await _unitOfWork.CourseSectionRepository.GetListAsync(
                s => s.CourseId == dto.CourseId);

            // Mevcut bölüm ID'lerini al
            var existingCourseSectionIds = existingCourseSections.Items.Select(s => s.Id).ToList();

            // Sadece yeni eklenecek bölümleri belirle (duplikasyonu önle)
            var newSectionIds = dto.SectionIds.Except(existingCourseSectionIds).ToList();

            if (!newSectionIds.Any())
                return new SuccessResult("All sections are already associated with this course.");

            // Yeni eklenecek bölümler için başlangıç sıra numarasını belirle
            int startOrderIndex = 0;

            if (existingCourseSections.Items.Any())
                startOrderIndex = existingCourseSections.Items.Max(s => s.OrderIndex) + 1;

            // Bölümleri tek sorguda getir
            var sectionsToAdd = await _unitOfWork.CourseSectionRepository.GetListAsync(
                s => newSectionIds.Contains(s.Id));

            // Yeni bölümleri kursa ekle
            int orderIndex = startOrderIndex;
            foreach (var sectionToAdd in sectionsToAdd.Items)
            {
                // Yeni bir bölüm oluştur
                var newSection = new CourseSection
                {
                    CourseId = dto.CourseId,
                    Title = sectionToAdd.Title,
                    Description = sectionToAdd.Description,
                    OrderIndex = orderIndex++
                };

                await _unitOfWork.CourseSectionRepository.AddAsync(newSection);
                await _unitOfWork.SaveChangesAsync();

                // Bu bölüme ait tüm videoları tek sorguda getir
                var sectionVideos = await _unitOfWork.SectionVideoRepository.GetListAsync(
                    sv => sv.CourseSectionId == sectionToAdd.Id,
                    orderBy: q => q.OrderBy(sv => sv.OrderIndex));

                // Videoları yeni bölüme ekle
                foreach (var sectionVideo in sectionVideos.Items)
                {
                    var newSectionVideo = new SectionVideo
                    {
                        CourseSectionId = newSection.Id,
                        VideoId = sectionVideo.VideoId,
                        OrderIndex = sectionVideo.OrderIndex
                    };

                    await _unitOfWork.SectionVideoRepository.AddAsync(newSectionVideo);
                }
            }

            await _unitOfWork.SaveChangesAsync();

            return new SuccessResult($"{newSectionIds.Count} section(s) added to the course.");
        }
        catch (Exception)
        {
            _unitOfWork.Rollback();
            throw;
        }
    }
    public async Task<IDataResult<CourseDto>> GetByIdAsync(int id)
    {
        var data = await _unitOfWork.CourseRepository.GetWithProjectionAsync(
            predicate: i => i.Id == id,
            selector: x => new CourseDto
            {
                Id = x.Id,
                Title = x.Title,
                Description = x.Description,
                ThumbnailUrl = x.ThumbnailUrl,
                IsActive = x.IsActive,
                Sections = x.Sections
                    .OrderBy(s => s.OrderIndex)
                    .Select(s => new CourseSectionDto
                    {
                        CourseId = s.CourseId,
                        Title = s.Title,
                        Description = s.Description,
                        OrderIndex = s.OrderIndex,
                        Videos = s.SectionVideos
                            .OrderBy(sv => sv.OrderIndex)
                            .Select(sv => new VideoDto
                            {
                                Id = sv.Video.Id,
                                Title = sv.Video.Title,
                                Description = sv.Video.Description,
                                ThumbnailUrl = sv.Video.ThumbnailUrl,
                                VideoUrl = sv.Video.VideoUrl,
                                DurationInSeconds = sv.Video.DurationInSeconds
                            }).ToList()
                    }).ToList()
            });

        if (data == null)
            throw new NotFoundException(MessageHelper.NotFound("Course"));

        return new SuccessDataResult<CourseDto>(data, MessageHelper.FetchedById("Course"));
    }
    public async Task<IDataResult<PaginatedResponse<CourseDto>>> GetVideoSectionsAsync(PageRequest pageRequest)
    {
        var courses = await _unitOfWork.CourseRepository.GetListWithProjectionAsync(
              selector: x => new CourseDto
              {
                  Id = x.Id,
                  Title = x.Title,
                  Description = x.Description,
                  ThumbnailUrl = x.ThumbnailUrl,
                  IsActive = x.IsActive,
                  Sections = x.Sections
                      .OrderBy(s => s.OrderIndex)
                      .Select(s => new CourseSectionDto
                      {
                          CourseId = s.CourseId,
                          Title = s.Title,
                          Description = s.Description,
                          OrderIndex = s.OrderIndex,
                          Videos = s.SectionVideos
                              .OrderBy(sv => sv.OrderIndex)
                              .Select(sv => new VideoDto
                              {
                                  Id = sv.Video.Id,
                                  Title = sv.Video.Title,
                                  Description = sv.Video.Description,
                                  ThumbnailUrl = sv.Video.ThumbnailUrl,
                                  VideoUrl = sv.Video.VideoUrl,
                                  DurationInSeconds = sv.Video.DurationInSeconds
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
        try
        {
            // Kursun var olup olmadığını kontrol et
            var course = await _unitOfWork.CourseRepository.GetAsync(c => c.Id == dto.CourseId);
            if (course == null)
                return new ErrorResult(MessageHelper.NotFound("Course"));

            if (dto.SectionIds == null || !dto.SectionIds.Any())
                return new ErrorResult("No section IDs provided.");

            // Kurs-bölüm ilişkilerini kontrol et
            var courseSections = await _unitOfWork.CourseSectionRepository.GetListAsync(
                s => s.CourseId == dto.CourseId && dto.SectionIds.Contains(s.Id));

            if (!courseSections.Items.Any())
                return new ErrorResult("None of the provided section IDs are associated with this course.");

            var foundSectionIds = courseSections.Items.Select(s => s.Id).ToList();
            var notFoundSectionIds = dto.SectionIds.Except(foundSectionIds).ToList();

            if (notFoundSectionIds.Any())
            {
                return new SuccessResult($"Warning: Section(s) with ID(s) {string.Join(", ", notFoundSectionIds)} were not found in this course.");
            }

            // Her bölüm için video ilişkilerini sil
            foreach (var section in courseSections.Items)
            {
                // Bölüme ait videoları al
                var sectionVideos = await _unitOfWork.SectionVideoRepository.GetListAsync(
                    sv => sv.CourseSectionId == section.Id);

                // Video ilişkilerini sil
                foreach (var sectionVideo in sectionVideos.Items)
                {
                    await _unitOfWork.SectionVideoRepository.HardDeleteAsync(sectionVideo);
                }

                // Bölümü sil
                await _unitOfWork.CourseSectionRepository.HardDeleteAsync(section);
            }

            await _unitOfWork.SaveChangesAsync();

            // Kalan bölümlerin sırasını güncelle
            var remainingSections = await _unitOfWork.CourseSectionRepository.GetListAsync(
                s => s.CourseId == dto.CourseId,
                orderBy: q => q.OrderBy(s => s.OrderIndex));

            if (remainingSections.Items.Any())
            {
                int orderIndex = 0;
                foreach (var section in remainingSections.Items)
                {
                    section.OrderIndex = orderIndex++;
                }
                await _unitOfWork.SaveChangesAsync();
            }

            return new SuccessResult($"{courseSections.Items.Count} section(s) removed from course.");
        }
        catch (Exception)
        {
            _unitOfWork.Rollback();
            throw;
        }
    }
}
