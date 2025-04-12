using AutoMapper;
using VideoCourse.Backend.Application.Features.CourseSections.DTOs;
using VideoCourse.Backend.Shared.Utils.Pagination;
using VideoCourse.Backend.Shared.Utils.Responses;

namespace VideoCourse.Application.Features.CourseSections.Profiles
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<IPaginate<CourseSectionDto>, PaginatedResponse<CourseSectionDto>>();
        }
    }
}
