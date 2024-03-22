using Core.CrossCuttingConcerns.Exceptions.Handlers;
using Microsoft.AspNetCore.Http;
using System.Net.Mime;

namespace Core.CrossCuttingConcerns.Exceptions.MiddlewareForException;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly HttpExceptionHandler _httpExceptionHandler;

    public ExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
        _httpExceptionHandler = new HttpExceptionHandler();
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context.Response, ex);
            
        }
    }

    private Task HandleExceptionAsync(HttpResponse response, System.Exception exception)
    {
        response.ContentType = MediaTypeNames.Application.Json;
        _httpExceptionHandler.Response = response;
        return _httpExceptionHandler.HandleExceptionAsync(exception);
    }

    //private Task LogException(HttpContext context, System.Exception exception)
    //{
    //    List<LogParameter> logParameters = [new LogParameter { Type = context.GetType().Name, Value = exception.ToString() }];

    //    LogDetail logDetail =
    //        new()
    //        {
    //            MethodName = _next.Method.Name,
    //            Parameters = logParameters,
    //            User = _contextAccessor.HttpContext?.User.Identity?.Name ?? "?"
    //        };

    //    _loggerService.Information(JsonSerializer.Serialize(logDetail));
    //    return Task.CompletedTask;
    //}
}
