using VideoCourse.Backend.Application.Features.Videos.DTOs;
using VideoCourse.Backend.Shared.Utils.Results.Abstract;
// Interfaces
public interface IS3Service
{
    Task<IDataResult<UploadedVideoResponse>> UploadUserVideo(UploadVideosDto dto);
    string GeneratePresignedUploadUrl(string fileName, string contentType);
}