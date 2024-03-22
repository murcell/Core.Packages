using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Core.CrossCuttingConcerns.Exceptions.Extensions;

public static class ProblemDetailsExtensions
{
    public static string AsJson<IProblemDetail>(this IProblemDetail details) where IProblemDetail : ProblemDetails=>JsonSerializer.Serialize(details);
}
