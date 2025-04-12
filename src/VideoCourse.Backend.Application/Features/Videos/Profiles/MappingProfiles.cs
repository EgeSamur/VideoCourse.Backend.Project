using AutoMapper;
using VideoCourse.Backend.Application.Features.Videos.DTOs;
using VideoCourse.Backend.Shared.Utils.Pagination;
using VideoCourse.Backend.Shared.Utils.Responses;

namespace VideoCourse.Application.Features.Videos.Profiles
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<IPaginate<VideoDto>, PaginatedResponse<VideoDto>>();
        }
    }
}
