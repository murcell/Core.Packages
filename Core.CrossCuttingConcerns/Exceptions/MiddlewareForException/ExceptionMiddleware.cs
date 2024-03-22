using Core.CrossCuttingConcerns.Exceptions.Handlers;
using Core.CrossCuttingConcerns.Logging;
using Core.CrossCuttingConcerns.Serilog;
using Microsoft.AspNetCore.Http;
using System.Diagnostics.Eventing.Reader;
using System.Net.Mime;
using static System.Net.Mime.MediaTypeNames;
using System.Text.Json;

namespace Core.CrossCuttingConcerns.Exceptions.MiddlewareForException;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly HttpExceptionHandler _httpExceptionHandler;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly LoggerServiceBase _loggerServiceBase;

    public ExceptionMiddleware(RequestDelegate next, IHttpContextAccessor httpContextAccessor, LoggerServiceBase loggerServiceBase)
    {
        _next = next;
        _httpExceptionHandler = new HttpExceptionHandler(); // burayı niye böyle dedik anlamadım burayı IOC'ye eklemeye bak
        _httpContextAccessor = httpContextAccessor;
        _loggerServiceBase = loggerServiceBase;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await LogException(context,ex);
            await HandleExceptionAsync(context.Response, ex);
            
        }
    }

    private Task LogException(HttpContext context, Exception ex)
    {
        List<LogParameter> logParameters = new()
        {
            new LogParameter(){ Type= context.GetType().Name, Value=ex.ToString() },
        };

        LogDetailWithException logDetail = new()
        {
            ExceptionMessage = ex.Message,
            MethodName = _next.Method.Name,
            Parameters = logParameters,
            User = _httpContextAccessor.HttpContext?.User.Identity?.Name ?? "?"
        };

        _loggerServiceBase.Error(JsonSerializer.Serialize(logDetail));

        return Task.CompletedTask;
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
