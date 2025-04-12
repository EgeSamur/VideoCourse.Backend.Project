namespace VideoCourse.Backend.Shared.Security.JWT;

/// <summary>
/// Provides methods for creating and validating JWT tokens.
/// </summary>
public interface ITokenHelper
{
    AccessToken CreateToken(int id, string[] roles, string[] permissions);
}