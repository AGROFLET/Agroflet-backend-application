using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Agroflet.Platform.Shared.Resources;
namespace Agroflet.Platform.Shared.Interfaces.Rest.Transform;

/// <summary>Maps application outcomes to localized REST ProblemDetails.</summary>
public static class ActionResultAssembler
{
    public static ActionResult FromResult<T>(this ControllerBase controller, Result<T> result, Func<T, ActionResult> onSuccess)
        => result.Match(onSuccess, error => Failure(controller, error));

    private static ActionResult Failure(ControllerBase controller, Error error)
    {
        var messages = controller.HttpContext.RequestServices.GetRequiredService<IStringLocalizer<ErrorMessages>>();
        var localized = messages[error.Code];
        var problem = new ProblemDetails
        {
            Status = error.Status,
            Title = error.Code,
            Detail = localized.ResourceNotFound ? messages[error.Status == 400 ? "validation-error" : "unexpected-error"].Value : localized.Value,
            Instance = controller.Request.Path,
            Extensions = { ["code"] = error.Code, ["traceId"] = controller.HttpContext.TraceIdentifier }
        };
        return new ObjectResult(problem) { StatusCode = error.Status, ContentTypes = { "application/problem+json" } };
    }
}
