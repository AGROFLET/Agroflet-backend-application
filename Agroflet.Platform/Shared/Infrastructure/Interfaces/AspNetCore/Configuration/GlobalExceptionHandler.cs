using Microsoft.Extensions.Localization;
using Agroflet.Platform.Shared.Resources;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace Agroflet.Platform.Shared.Infrastructure.Interfaces.AspNetCore.Configuration;

public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger, IStringLocalizer<ErrorMessages> messages) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext context, Exception exception, CancellationToken cancellationToken)
    {
        var (status, code) = exception switch
        {
            BusinessException e => (e.Status, e.Message),
            ArgumentException e => (400, e.Message),
            DbUpdateConcurrencyException => (409, "concurrent-update-retry"),
            DbUpdateException e when IsConstraint(e) => (409, "duplicate-or-invalid-reference"),
            _ => (500, "unexpected-error")
        };
        if (status == 500) logger.LogError(exception, "Unhandled request failure");
        context.Response.StatusCode = status;
        var message = messages[code];
        context.Response.ContentType = "application/problem+json";
        var problem = new ProblemDetails
        {
            Status = status,
            Title = code,
            Detail = message.ResourceNotFound ? messages[status == 400 ? "validation-error" : "unexpected-error"].Value : message.Value,
            Instance = context.Request.Path,
            Extensions = { { "code", code }, { "traceId", context.TraceIdentifier } }
        };
        await context.Response.WriteAsJsonAsync(problem, options: (System.Text.Json.JsonSerializerOptions?)null,
            contentType: "application/problem+json", cancellationToken: cancellationToken);
        return true;
    }
    private static bool IsConstraint(Exception exception)
    {
        for (Exception? e = exception; e != null; e = e.InnerException)
            if (e.GetType().GetProperty("Number")?.GetValue(e) is int number && number is 1062 or 1451 or 1452 or 1213) return true;
        return false;
    }
}
