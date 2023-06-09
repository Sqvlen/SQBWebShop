﻿using System.Net;
using System.Text.Json;
using API.Errors;

namespace API.Middleware;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _requestDelegate;
    private readonly ILogger<ExceptionMiddleware> _logger;
    private readonly IHostEnvironment _environment;

    public ExceptionMiddleware(RequestDelegate requestDelegate, 
        ILogger<ExceptionMiddleware> logger,
        IHostEnvironment environment)
    {
        _requestDelegate = requestDelegate;
        _logger = logger;
        _environment = environment;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _requestDelegate(context);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, exception.Message);
            
            
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var response = _environment.IsDevelopment()
                ? new ApiException(
                    (int)HttpStatusCode.InternalServerError, exception.Message,
                    exception.StackTrace.ToString())
                : new ApiResponse((int)HttpStatusCode.InternalServerError);

            var options = new JsonSerializerOptions{PropertyNamingPolicy = JsonNamingPolicy.CamelCase};
            
            var json = JsonSerializer.Serialize(response, options);
            await context.Response.WriteAsync(json);
        }
    }
}