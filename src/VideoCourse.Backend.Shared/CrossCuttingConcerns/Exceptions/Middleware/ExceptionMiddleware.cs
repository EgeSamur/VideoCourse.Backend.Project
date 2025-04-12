using System.Text.Json;
using VideoCourse.Backend.Shared.CrossCuttingConcerns.Exceptions.Handlers;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using VideoCourse.Backend.Shared.Security.Extensions;

namespace VideoCourse.Backend.Shared.CrossCuttingConcerns.Exceptions.Middleware;

public class ExceptionMiddleware
{
    private readonly HttpExceptionHandler _httpExceptionHandler;
    private readonly ILogger<ExceptionMiddleware> _logger;
    private readonly RequestDelegate _next;

    private readonly JsonSerializerOptions _jsonSerializerOptions = new JsonSerializerOptions
    {
        Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    };

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
    {
        _next = next;
        _httpExceptionHandler = new HttpExceptionHandler();
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        var endpoint = context.Request.Path.Value;
        var userId = context.User.GetUserId()?.ToString() ?? "Anonymous";
        var ipAddress = context.Connection.RemoteIpAddress;
        var correlationId = context.TraceIdentifier;
        var userAgent = context.Request.Headers["User-Agent"].ToString();
        var requestUrl = $"{context.Request.Scheme}://{context.Request.Host}{context.Request.Path}{context.Request.QueryString}";
        var httpMethod = context.Request.Method;
        var requestHeaders = JsonSerializer.Serialize(context.Request.Headers, _jsonSerializerOptions);
        var requestBody = await GetBodyAsync(context.Request);
        string responseBody = string.Empty;
        
        var originalBodyStream = context.Response.Body;
        bool isError = false;
        
        try
        {
            using var memoryStream = new MemoryStream();
            context.Response.Body = memoryStream;

            _logger.LogInformation(
                @"Handling {Endpoint} 
                UserId: {UserId}, 
                IP: {ClientIp}, 
                CorrelationId: {CorrelationId}, 
                UserAgent: {UserAgent}, 
                RequestUrl: {RequestUrl}, 
                HttpMethod: {HttpMethod}, 
                Headers: {Headers}, 
                RequestBody: {RequestBody}",
                endpoint, userId, ipAddress, correlationId, userAgent, requestUrl, httpMethod, requestHeaders, requestBody
            );
            
            try 
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                isError = true;
                await HandleExceptionAsync(context.Response, ex);
            }
            
            memoryStream.Position = 0;
            responseBody = await new StreamReader(memoryStream).ReadToEndAsync();
            
            memoryStream.Position = 0;
            await memoryStream.CopyToAsync(originalBodyStream);
        }
        finally
        {
            context.Response.Body = originalBodyStream;
            
            if (isError)
            {
                _logger.LogError(
                    @"Handled {Endpoint} 
                    UserId: {UserId}, 
                    IP: {ClientIp}, 
                    CorrelationId: {CorrelationId}, 
                    UserAgent: {UserAgent}, 
                    RequestUrl: {RequestUrl}, 
                    HttpMethod: {HttpMethod}, 
                    Headers: {Headers}, 
                    RequestBody: {RequestBody},
                    ResponseBody: {ResponseBody}",
                    endpoint, userId, ipAddress, correlationId, userAgent, requestUrl, httpMethod, requestHeaders, requestBody, responseBody
                );
            }
            else
            {
                _logger.LogInformation(
                    @"Handled {Endpoint} 
                    UserId: {UserId}, 
                    IP: {ClientIp}, 
                    CorrelationId: {CorrelationId}, 
                    UserAgent: {UserAgent}, 
                    RequestUrl: {RequestUrl}, 
                    HttpMethod: {HttpMethod}, 
                    Headers: {Headers}, 
                    RequestBody: {RequestBody},
                    ResponseBody: {ResponseBody}",
                    endpoint, userId, ipAddress, correlationId, userAgent, requestUrl, httpMethod, requestHeaders, requestBody, responseBody
                );
            }
        }
    }

    private Task HandleExceptionAsync(HttpResponse response, Exception exception)
    {
        response.ContentType = "application/json";
        _httpExceptionHandler.Response = response;
        return _httpExceptionHandler.HandleExceptionAsync(exception);
    }
    
    private async Task<string> GetBodyAsync(HttpRequest request)
    {
        request.EnableBuffering();

        using var reader = new StreamReader(
            request.Body,
            encoding: System.Text.Encoding.UTF8,
            detectEncodingFromByteOrderMarks: false,
            leaveOpen: true);

        var body = await reader.ReadToEndAsync();
        request.Body.Position = 0;

        return body;
    }
}