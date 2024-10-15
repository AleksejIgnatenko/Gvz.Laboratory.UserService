using System.Diagnostics;


public class LoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<LoggingMiddleware> _logger;

    public LoggingMiddleware(RequestDelegate next, ILogger<LoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        var sw = Stopwatch.StartNew();

        var requestTime = DateTime.Now;
        _logger.LogInformation($"Request at: {requestTime}, id request: {context.TraceIdentifier}, method: {context.Request.Method}, path: {context.Request.Path}");

        context.Items["RequestStartTime"] = requestTime;

        await _next(context);

        sw.Stop();
        var elapsed = sw.ElapsedMilliseconds;

        var responseTime = DateTime.Now;
        _logger.LogInformation($"Response at: {responseTime}, request at: {requestTime}, id request: {context.TraceIdentifier} status code: {context.Response.StatusCode} ({elapsed} ms)");
    }
}

