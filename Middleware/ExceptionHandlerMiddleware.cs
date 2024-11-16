using Gvz.Laboratory.UserService.Exceptions;
using Serilog;
using System.Text.Json;

public class ExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionHandlerMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (UserValidationException ex)
        {
            var statusCode = StatusCodes.Status400BadRequest;

            context.Response.StatusCode = statusCode;
            context.Response.ContentType = "application/json";

            var result = JsonSerializer.Serialize(new { error = ex.Errors });
            await context.Response.WriteAsync(result);
        }
        catch (AuthenticationFailedException ex)
        {
            var statusCode = StatusCodes.Status401Unauthorized;

            context.Response.StatusCode = statusCode;
            context.Response.ContentType = "application/json";

            var result = JsonSerializer.Serialize(new { error = ex.Message });
            await context.Response.WriteAsync(result);
        }
        catch (RepositoryException ex)
        {
            var statusCode = StatusCodes.Status409Conflict;

            context.Response.StatusCode = statusCode;
            context.Response.ContentType = "application/json";

            var result = JsonSerializer.Serialize(new { error = ex.Message });
            await context.Response.WriteAsync(result);
        }
        catch (InvalidOperationException ex)
        {
            var statusCode = StatusCodes.Status404NotFound;

            context.Response.StatusCode = statusCode;
            context.Response.ContentType = "application/json";

            var result = JsonSerializer.Serialize(new { error = ex.Message });
            await context.Response.WriteAsync(result);
        }
        catch (Exception ex)
        {
            Log.Error(ex.Message);
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            context.Response.ContentType = "application/json";

            var result = JsonSerializer.Serialize(new { error = ex.Message });
            await context.Response.WriteAsync(result);
        }
    }
}
