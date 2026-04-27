using QDryClean.Application.Common.Errors;
using QDryClean.Application.Common.Exceptions;
using QDryClean.Application.Common.Responses;

namespace QDryClean.Api.Middlewares;

public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ErrorHandlingMiddleware> _logger;

    public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error occurred");

            context.Response.ContentType = "application/json";

            var code = 0;

            if (ex is BaseException appEx)
                code = appEx.Code;
            else code = ErrorCodes.InternalServerError;

            context.Response.StatusCode = ex switch
            {
                NotFoundException => StatusCodes.Status404NotFound,
                BadRequestExeption => StatusCodes.Status400BadRequest,
                InvalidOperationException => StatusCodes.Status400BadRequest,
                UnauthorizedAccessException => StatusCodes.Status401Unauthorized,
                InvalidLoginOrPasswordException => StatusCodes.Status401Unauthorized,
                _ => StatusCodes.Status500InternalServerError
            };
            var message = ex.InnerException?.Message ?? ex.Message;

            await context.Response.WriteAsJsonAsync(ApiResponseFactory.Fail<object>(code, message));
        }
    }
}