using Microsoft.AspNetCore.Http;

namespace VideoCourse.Backend.Application.Features.Videos.DTOs;
public class UploadVideosDto
{
    public List<IFormFile> Files { get; set; }
}

public class UploadedVideoResponseDto
{
    public string Url { get; set; }
    public bool IsVideo { get; set; }
}

public class UploadedVideoResponse
{
    public List<UploadedVideoResponseDto> Medias { get; set; }
}