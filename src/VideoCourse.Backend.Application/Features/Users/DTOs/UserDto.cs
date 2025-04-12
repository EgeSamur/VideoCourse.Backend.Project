namespace VideoCourse.Backend.Application.Features.Users.DTOs;

public class UserDto
{
    public int Id { get; set; }
    public string FullName { get; set; } = String.Empty;
    public string Email { get; set; } = String.Empty;
    public string? PhoneNumber { get; set; } = null;
    public string? Job { get; set; } = null;
    public string? ProfileImageUrl { get; set; } = null;
    public bool IsAdmin { get; set; } = false;
}
