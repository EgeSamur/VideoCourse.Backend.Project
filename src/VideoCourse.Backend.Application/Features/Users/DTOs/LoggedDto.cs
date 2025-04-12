using VideoCourse.Backend.Shared.Security.JWT;

namespace VideoCourse.Backend.Application.Features.Users.DTOs;

public class LoggedDto
{
    public int UserId { get; set; }
    public AccessToken AccessToken { get; set; } = new AccessToken();

}
