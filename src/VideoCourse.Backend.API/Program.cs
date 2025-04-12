using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using VideoCourse.Backend.Application;
using VideoCourse.Backend.Infrastructure;
using VideoCourse.Backend.Shared.CrossCuttingConcerns.Exceptions.Extensions;
using VideoCourse.Backend.Shared.Security;
using VideoCourse.Backend.Shared.Security.Encryption;
using VideoCourse.Backend.Shared.Security.JWT;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", b =>
    {
        b.AllowAnyHeader()
            .AllowAnyMethod()
            .SetIsOriginAllowed((host) => true)
            .AllowCredentials();
    });
});

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddHttpContextAccessor();
builder.Services.AddSecurityServices();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(opt =>
{
    opt.AddSecurityDefinition(
        name: "Bearer",
        securityScheme: new OpenApiSecurityScheme
        {
            Name = "Authorization",
            Type = SecuritySchemeType.ApiKey,
            Scheme = "Bearer",
            BearerFormat = "JWT",
            In = ParameterLocation.Header,
            Description =
                "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345.54321\""
        }
    );
    opt.AddSecurityRequirement(
        new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
                },
                new string[] { }
            }
        }
    );
});


var tokenOptions = builder.Configuration.GetSection("TokenOptions").Get<TokenOptions>();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidIssuer = tokenOptions!.Issuer,
            ValidAudience = tokenOptions.Audience,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = SecurityKeyHelper.CreateSecurityKey(tokenOptions.SecurityKey),
            ClockSkew = TimeSpan.Zero
        };
    });



Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Fatal()
    .MinimumLevel.Override("VideoCourse.Backend.Shared.CrossCuttingConcerns.Exceptions.Middleware", Serilog.Events.LogEventLevel.Verbose) // LoggingBehavior için en düşük seviye
    
    .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day, encoding: System.Text.Encoding.UTF8)
    // .WriteTo.Seq("http://localhost:5341") // Seq için
    .Enrich.FromLogContext() // Log context'i zenginleştirir
    .CreateLogger();

builder.Host.UseSerilog();

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("Redis");
});


var app = builder.Build();
#region Hangifire
// Hangfire Job Planlama

//// Use Hangfire Dashboard
//app.UseHangfireDashboard("/hangfire");

//using (var scope = app.Services.CreateScope())
//{
//    var notificationService = scope.ServiceProvider.GetRequiredService<INotificationService>();
//    var backgroundService = scope.ServiceProvider.GetRequiredService<IBackgroundService>();
//    RecurringJob.AddOrUpdate(
//        "SendWelcomeMessage", // Job ad�
//        () => notificationService.SendWelcomeMessageAsync(),
//        Cron.MinuteInterval(20));

//    RecurringJob.AddOrUpdate(
//    "SendComeBackMessageDaily", // Job ad�
//    () => notificationService.SendComeBackMessageAsync(),
//    Cron.Daily());

//    RecurringJob.AddOrUpdate(
//    "SendWeeklyBuyPremium",
//    () => notificationService.SendBuyPremiumMessageAsync(),
//    Cron.Weekly());

//    RecurringJob.AddOrUpdate(
//      "ResetSwipeRight",
//      () => backgroundService.ResetSwipeRightsAsync(),
//      Cron.Hourly()); // Her 12 saatte bir �al��t�r

//    RecurringJob.AddOrUpdate(
//        "UpdateUserPackages",
//        () => backgroundService.UpdateUserPackagesAsync(),
//        Cron.Hourly()); // Her saat ba�� �al��t�r
//    RecurringJob.AddOrUpdate(
//    "ValidateUserVideos",
//    () => backgroundService.ValidateUserVideosAsync(),
//    Cron.MinuteInterval(15)); // Run every 15 minutes

//    RecurringJob.AddOrUpdate(
//   "CheckOnDateMode",
//   () => backgroundService.CheckDateModeTime(),
//   Cron.MinuteInterval(15)); // Run every 15 minutes

//}
#endregion



// Configure the HTTP request pipeline.
// if (app.Environment.IsDevelopment())
// {
//     app.UseSwagger();
//     app.UseSwaggerUI();
// }

app.UseSwagger();
app.UseSwaggerUI();

app.ConfigureCustomExceptionMiddleware();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseCors("AllowAll");
app.UseAuthorization();
app.MapControllers();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers(); 
});


app.Run();