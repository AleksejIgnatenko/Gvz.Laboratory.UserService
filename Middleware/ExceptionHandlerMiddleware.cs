using Gvz.Laboratory.UserService.Exceptions;
using System.Text.Json;

public class ExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlerMiddleware> _logger;

    public ExceptionHandlerMiddleware(RequestDelegate next, ILogger<ExceptionHandlerMiddleware> logger)
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
        catch (UserValidationException ex)
        {
            var requestTime = context.Items["RequestStartTime"] as DateTime? ?? DateTime.Now;
            var statusCode = StatusCodes.Status400BadRequest;

            _logger.LogError($"Response at: {DateTime.Now}, request at: {requestTime}, id request: {context.TraceIdentifier} status code: {statusCode}\n"
                + string.Join(",\n", ex.Errors.Select(x => x.Key + ": " + x.Value)));

            context.Response.StatusCode = statusCode;
            context.Response.ContentType = "application/json";

            var result = JsonSerializer.Serialize(new { error = ex.Errors });
            await context.Response.WriteAsync(result);
        }
        catch (UsersRepositoryException ex)
        {
            var requestTime = context.Items["RequestStartTime"] as DateTime? ?? DateTime.Now;
            var statusCode = StatusCodes.Status409Conflict;

            _logger.LogError($"Response at: {DateTime.Now}, request at: {requestTime}, id request: {context.TraceIdentifier} status code: {statusCode}\n"
                + ex.Message);

            context.Response.StatusCode = statusCode;
            context.Response.ContentType = "application/json";

            var result = JsonSerializer.Serialize(new { error = ex.Message });
            await context.Response.WriteAsync(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);

            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            context.Response.ContentType = "application/json";

            var result = JsonSerializer.Serialize(new { error = ex.Message });
            await context.Response.WriteAsync(result);
        }
    }
}
