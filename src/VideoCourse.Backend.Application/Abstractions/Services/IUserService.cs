using VideoCourse.Backend.Application.Features.Users.DTOs;
using VideoCourse.Backend.Shared.Utils.Requests;
using VideoCourse.Backend.Shared.Utils.Responses;
using VideoCourse.Backend.Shared.Utils.Results.Abstract;

public interface IUserService
{
    Task<IDataResult<LoggedDto>> CreateAsync(CreateUserDto dto);
    Task<IDataResult<UserDto>> GetByIdAsync(int id);
    Task<IDataResult<PaginatedResponse<UserDto>>> GetUsers(PageRequest pageRequest);
    Task<IDataResult<LoggedDto>> LoginAsync(UserLoginDto dto);
    Task<IResult> DeleteAsync(int id);
}
