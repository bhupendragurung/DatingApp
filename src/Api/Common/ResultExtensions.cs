using Domain.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Api.Common;

public static class ResultExtensions
{
    public static ActionResult ToProblem(this ControllerBase controller, Error error)
    {
        if (error is { Type: ErrorType.Validation, ValidationErrors: not null })
        {
            var modelState = new ModelStateDictionary();
            foreach (var (field, messages) in error.ValidationErrors)
            {
                foreach (var message in messages)
                {
                    modelState.AddModelError(field, message);
                }
            }

            return controller.ValidationProblem(modelState);
        }

        var status = error.Type switch
        {
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            ErrorType.Conflict => StatusCodes.Status409Conflict,
            _ => StatusCodes.Status400BadRequest
        };

        var problem = controller.ProblemDetailsFactory.CreateProblemDetails(
            controller.HttpContext, statusCode: status, title: error.Message);
        problem.Extensions["code"] = error.Code;

        return new ObjectResult(problem)
        {
            StatusCode = status,
            ContentTypes = { "application/problem+json" }
        };
    }
}