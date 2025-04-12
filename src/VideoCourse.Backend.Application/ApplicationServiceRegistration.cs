using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using Amazon;
using Amazon.Extensions.NETCore.Setup;
using Amazon.Runtime;
using Amazon.S3;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VideoCourse.Backend.Application.Common.DTOs;
using VideoCourse.Backend.Application.Features.Courses;
using VideoCourse.Backend.Application.Features.CourseSections;
using VideoCourse.Backend.Application.Features.Users;
using VideoCourse.Backend.Application.Features.Videos;

namespace VideoCourse.Backend.Application;

public static class ApplicationServiceRegistration
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        services.AddSingleton<JsonSerializerOptions>(new JsonSerializerOptions
        {
            // Allow positive and negative infinity literals
            NumberHandling = JsonNumberHandling.AllowNamedFloatingPointLiterals
        });

        services.AddHttpClient();

        var awsOptions = configuration.GetSection("AwsConfigs").Get<AwsConfiguration>();
        services.AddAWSService<IAmazonS3>(new AWSOptions
        {
            Region = RegionEndpoint.EUCentral1,
            Credentials = new BasicAWSCredentials(awsOptions!.AccessKey, awsOptions!.SecretKey)
        });
        services.Configure<AwsConfiguration>(configuration.GetSection("AwsConfigs"));

        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IVideoService, VideoService>();
        services.AddScoped<ICourseSectionService, CourseSectionService>();
        services.AddScoped<ICourseService, CourseService>();

        return services;
    }
}