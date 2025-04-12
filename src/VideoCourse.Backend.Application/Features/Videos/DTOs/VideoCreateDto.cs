namespace VideoCourse.Backend.Application.Features.Videos.DTOs;

public class VideoCreateDto
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; } = string.Empty;
    public string? ThumbnailUrl { get; set; } = string.Empty;
    public int DurationInSeconds { get; set; }
    // S3 URL simule edilmiş
    public string VideoUrl { get; set; } = string.Empty;
}

