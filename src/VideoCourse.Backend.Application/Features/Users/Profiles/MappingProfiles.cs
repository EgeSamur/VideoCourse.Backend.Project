using AutoMapper;
using VideoCourse.Backend.Application.Features.Users.DTOs;
using VideoCourse.Backend.Shared.Utils.Pagination;
using VideoCourse.Backend.Shared.Utils.Responses;

namespace VideoCourse.Application.Features.Users.Profiles
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<IPaginate<UserDto>, PaginatedResponse<UserDto>>();
            //CreateMap<IPaginate<UserCardDto>, PaginatedResponse<UserCardDto>>();
            //CreateMap<IPaginate<UserLikesDto>, PaginatedResponse<UserLikesDto>>();
            //CreateMap<IPaginate<UserMatchesDto>, PaginatedResponse<UserMatchesDto>>();
            //CreateMap<IPaginate<UserRightDto>, PaginatedResponse<UserRightDto>>();
        }
    }
}
