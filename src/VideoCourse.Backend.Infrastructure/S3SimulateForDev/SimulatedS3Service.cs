using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using VideoCourse.Backend.Application.Features.Videos.DTOs;
using VideoCourse.Backend.Shared.CrossCuttingConcerns.Exceptions.Types;
using VideoCourse.Backend.Shared.Utils.Results.Abstract;
using VideoCourse.Backend.Shared.Utils.Results.Concrete;

namespace VideoCourse.Backend.Infrastructure.S3SimulateForDev;

public class SimulatedS3Service : IS3Service
{
    private readonly IConfiguration _configuration;
    private readonly string _localStoragePath;
    private readonly string _s3BaseUrl;

    public SimulatedS3Service(IConfiguration configuration)
    {
        _configuration = configuration;
        // Create a directory in the app path to simulate S3 storage
        _localStoragePath = Path.Combine(Directory.GetCurrentDirectory(), "SimulatedS3Storage");
        if (!Directory.Exists(_localStoragePath))
        {
            Directory.CreateDirectory(_localStoragePath);
        }
        // Set a fake S3 URL base for simulating responses
        _s3BaseUrl = "https://simulated-s3-bucket.s3.amazonaws.com/";
    }

    public async Task<IDataResult<UploadedVideoResponse>> UploadUserVideo(UploadVideosDto dto)
    {
        try
        {
            var videos = dto.Files;
            var result = new UploadedVideoResponse()
            {
                Medias = new List<UploadedVideoResponseDto>()
            };

            foreach (var video in videos)
            {
                if (video.ContentType.StartsWith("video/"))
                {
                    #region Video Control
                    if (!video.ContentType.StartsWith("video/"))
                        throw new BusinessException("The file must be a video.");
                    if (video.Length > 100 * 1024 * 1024) // 100 MB limit
                        throw new BusinessException("Video size must be less than 100 MB.");
                    #endregion

                    #region Video Processing
                    // Log the video information instead of actually processing it

                    // Simulate a slight delay as if we're processing the video
                    await Task.Delay(500);
                    #endregion

                    //#region Simulate Upload to S3
                    //// Generate a unique filename
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(video.FileName);
                    var filePath = Path.Combine(_localStoragePath, fileName);

                    //// Optionally save the file locally to simulate storage (can be commented out to save disk space)
                    //// This step is optional - you can just log and pretend without actual file saving
                    //if (_configuration.GetValue<bool>("SimulateActualFileStorage", false))
                    //{
                    //    using (var stream = new FileStream(filePath, FileMode.Create))
                    //    {
                    //        await video.CopyToAsync(stream);
                    //    }
                    //    _logger.LogInformation($"Simulated S3 upload - File saved locally at: {filePath}");
                    //}
                    //else
                    //{
                    //    _logger.LogInformation($"Simulated S3 upload without saving file: {fileName}");
                    //}

                    //// Simulate some processing time for the upload
                    //await Task.Delay(video.Length > 10 * 1024 * 1024 ? 2000 : 1000);
                    //#endregion

                    // Create a fake S3 URL for the uploaded video
                    var fakeS3Url = _s3BaseUrl + fileName;

                    var uploadedVideo = new UploadedVideoResponseDto()
                    {
                        IsVideo = true,
                        Url = fakeS3Url
                    };
                    result.Medias.Add(uploadedVideo);
                }
                else
                {
                }
            }

            return new SuccessDataResult<UploadedVideoResponse>(result, "Videos uploaded successfully");
        }
        catch (BusinessException bex)
        {
            return new ErrorDataResult<UploadedVideoResponse>(bex.Message);
        }
        catch (Exception ex)
        {
            return new ErrorDataResult<UploadedVideoResponse>("An error occurred during video upload");
        }
    }

    // Generate a pre-signed URL for client-side uploading
    public string GeneratePresignedUploadUrl(string fileName, string contentType)
    {
        // In a real implementation, this would create an S3 pre-signed URL
        // For simulation, we'll just return a fake URL that looks like a pre-signed URL
        var fakePreSignedUrl = $"https://simulated-s3-bucket.s3.amazonaws.com/{Guid.NewGuid()}/{fileName}?X-Amz-Algorithm=AWS4-HMAC-SHA256&X-Amz-Credential=AKIAEXAMPLE%2F20220318%2Fus-east-1%2Fs3%2Faws4_request&X-Amz-Date=20220318T180513Z&X-Amz-Expires=3600&X-Amz-SignedHeaders=host&X-Amz-Signature=fakeSignature123456789";

        return fakePreSignedUrl;
    }
}

