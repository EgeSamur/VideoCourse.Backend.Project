namespace VideoCourse.Backend.Application.Features.Videos.DTOs;

public class VideoDto
{
    public int Id { get; set; }
    public string Title { get; set; } = String.Empty;
    public string Description { get; set; } = String.Empty;
    public string VideoUrl { get; set; } = String.Empty;
    public int DurationInSeconds { get; set; }
    public string ThumbnailUrl { get; set; } = String.Empty;
}
