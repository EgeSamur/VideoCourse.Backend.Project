using AutoMapper;
using VideoCourse.Backend.Application.Features.Courses.DTOs;
using VideoCourse.Backend.Shared.Utils.Pagination;
using VideoCourse.Backend.Shared.Utils.Responses;

namespace VideoCourse.Application.Features.Courses.Profiles
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<IPaginate<CourseDto>, PaginatedResponse<CourseDto>>();
        }
    }
}
