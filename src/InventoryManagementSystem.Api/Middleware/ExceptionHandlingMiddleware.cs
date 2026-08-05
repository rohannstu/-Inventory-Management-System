using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace InventoryManagementSystem.Api.Middleware;

public class ExceptionHandlingMiddleware(
    RequestDelegate next,
    ILogger<ExceptionHandlingMiddleware> logger,
    IHostEnvironment environment)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception exception)
        {
            await HandleExceptionAsync(context, exception);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        if (context.Response.HasStarted)
        {
            logger.LogWarning(exception, "An exception occurred after the response had already started.");
            throw exception;
        }

        logger.LogError(exception, "An unhandled exception occurred while processing the request.");

        context.Response.ContentType = "application/problem+json";

        var problemDetails = exception switch
        {
            ValidationException validationException => CreateValidationProblemDetails(context, validationException),
            UnauthorizedAccessException => CreateProblemDetails(
                context,
                StatusCodes.Status401Unauthorized,
                "Unauthorized",
                exception.Message),
            ArgumentException => CreateProblemDetails(
                context,
                StatusCodes.Status400BadRequest,
                "Bad Request",
                exception.Message),
            InvalidOperationException when IsConflict(exception) => CreateProblemDetails(
                context,
                StatusCodes.Status409Conflict,
                "Conflict",
                exception.Message),
            InvalidOperationException => CreateProblemDetails(
                context,
                StatusCodes.Status400BadRequest,
                "Bad Request",
                exception.Message),
            _ => CreateProblemDetails(
                context,
                StatusCodes.Status500InternalServerError,
                "Internal Server Error",
                "An unexpected error occurred.")
        };

        context.Response.StatusCode = problemDetails.Status ?? StatusCodes.Status500InternalServerError;

        if (environment.IsDevelopment())
        {
            problemDetails.Extensions["exception"] = exception.GetType().Name;
            problemDetails.Extensions["traceId"] = context.TraceIdentifier;
        }

        await context.Response.WriteAsJsonAsync(problemDetails);
    }

    private static ValidationProblemDetails CreateValidationProblemDetails(
        HttpContext context,
        ValidationException exception)
    {
        var errors = exception.Errors
            .GroupBy(error => error.PropertyName)
            .ToDictionary(
                group => group.Key,
                group => group.Select(error => error.ErrorMessage).ToArray());

        return new ValidationProblemDetails(errors)
        {
            Status = StatusCodes.Status400BadRequest,
            Title = "Validation Failed",
            Detail = "One or more validation errors occurred.",
            Instance = context.Request.Path
        };
    }

    private static ProblemDetails CreateProblemDetails(
        HttpContext context,
        int statusCode,
        string title,
        string detail)
    {
        return new ProblemDetails
        {
            Status = statusCode,
            Title = title,
            Detail = detail,
            Instance = context.Request.Path
        };
    }

    private static bool IsConflict(Exception exception)
    {
        return exception.Message.Contains("already exists", StringComparison.OrdinalIgnoreCase);
    }
}
