using Microsoft.AspNetCore.Builder;
using Core.CrossCuttingConcerns.Exceptions.MiddlewareForException;
namespace Core.CrossCuttingConcerns.Exceptions.Extensions;

public static class ExceptionMiddlewareExtensions
{
    public static void ConfigureCustomExceptionMiddleware(this IApplicationBuilder app)=> app.UseMiddleware<ExceptionMiddleware>();
}
