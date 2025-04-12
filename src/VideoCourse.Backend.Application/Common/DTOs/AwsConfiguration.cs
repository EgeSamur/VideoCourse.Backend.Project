namespace VideoCourse.Backend.Application.Common.DTOs;
public class AwsConfiguration
{
    public string AccessKey { get; set; }
    public string SecretKey { get; set; }
    public string UserImagesBucketName { get; set; }
    public string TempImagesBucketName { get; set; }
    public string IconsBucketName { get; set; }
    public string Region { get; set; }
}