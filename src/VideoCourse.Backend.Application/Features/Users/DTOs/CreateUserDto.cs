namespace VideoCourse.Backend.Application.Features.Users.DTOs;
public class CreateUserDto
{
    public string FullName { get; set; } = String.Empty;
    public string Email { get; set; } = String.Empty;
    public string Password { get; set; } = String.Empty;
    public string? PhoneNumber { get; set; } = null;
    public string? Job { get; set; } = null;
}
