using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using VideoCourse.Backend.Application.Abstractions.Repositories;
using VideoCourse.Backend.Infrastructure.Persistence.Contexts;
using VideoCourse.Backend.Infrastructure.Persistence.Interceptors;
using VideoCourse.Backend.Infrastructure.Persistence.Repositories;
using VideoCourse.Backend.Infrastructure.Persistence.Repositories.Base;
using VideoCourse.Backend.Infrastructure.S3SimulateForDev;

namespace VideoCourse.Backend.Infrastructure;

public static class InfrastructureServiceRegistration
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<ISaveChangesInterceptor, EntitySaveChangesInterceptor>();
        // ConnectionString'den DataSource olu�turma
        var connectionString = configuration.GetConnectionString("Default");
        var dataSourceBuilder = new NpgsqlDataSourceBuilder(connectionString);
        dataSourceBuilder.UseNetTopologySuite(); // NetTopologySuite deste�ini ekliyoruz
        var dataSource = dataSourceBuilder.Build();

        services.AddDbContext<ApplicationDbContext>((sp, options) =>
        {
            options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());
            options.UseNpgsql(dataSource, npgsqlOptions =>
            {
                npgsqlOptions.UseNetTopologySuite(); // NetTopologySuite'i kullan�yoruz
            });
        });
        services.AddScoped<IUnitOfWork, UnitOfWork>();


        #region AmazonRekognition
        //// AWS yapılandırmalarını doğrudan parametre olarak veriyoruz
        //var accessKey = configuration["AwsConfigs:AccessKey"];
        //var secretKey = configuration["AwsConfigs:SecretKey"];
        //var region = configuration["AwsConfigs:Region"];

        //var awsCredentials = new BasicAWSCredentials(accessKey, secretKey);
        //var rekognitionConfig = new AmazonRekognitionConfig
        //{
        //    RegionEndpoint = Amazon.RegionEndpoint.GetBySystemName(region)
        //};

        //services.AddSingleton<IAmazonRekognition>(sp => new AmazonRekognitionClient(awsCredentials, rekognitionConfig));
        ////services.AddScoped<IImageVerificationService, RekognitionImageVerificationService>();
        #endregion

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ICourseRepository, CourseRepository>();
        services.AddScoped<ICourseSectionRepository, CourseSectionRepository>();
        services.AddScoped<ICourseSectionVideoRepository, CourseSectionVideoRepository>();
        services.AddScoped<IPaymentRepository, PaymentRepository>();
        services.AddScoped<IUserCourseRepository, UserCourseRepository>();
        services.AddScoped<IVideoProgressRepository, VideoProgressRepository>();
        services.AddScoped<IVideoRepository, VideoRepository>();
        services.AddScoped<ICourseCourseSectionRepository, CourseCourseSectionRepository>();
        
        
        
        services.AddScoped<IS3Service, SimulatedS3Service>();

        return services;
    }
}