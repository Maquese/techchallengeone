using System.Diagnostics;

namespace AutoReparaAPI.Middleware;

public class CorrelationMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<CorrelationMiddleware> _logger;

    public CorrelationMiddleware(RequestDelegate next, ILogger<CorrelationMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var correlationId = context.Request.Headers["X-Correlation-ID"].FirstOrDefault();
        if (string.IsNullOrWhiteSpace(correlationId))
        {
            correlationId = Guid.NewGuid().ToString();
        }

        context.TraceIdentifier = correlationId;
        context.Response.Headers["X-Correlation-ID"] = correlationId;
        context.Items["CorrelationId"] = correlationId;

        if (Activity.Current == null)
        {
            var activity = new Activity("AutoReparaAPI.Request");
            activity.SetTag("correlation_id", correlationId);
            activity.SetTag("http.method", context.Request.Method);
            activity.SetTag("http.path", context.Request.Path.ToString());
            activity.Start();
        }
        else
        {
            Activity.Current?.SetTag("correlation_id", correlationId);
        }

        using (_logger.BeginScope(new Dictionary<string, object>
        {
            ["correlation_id"] = correlationId,
            ["request_id"] = context.TraceIdentifier,
            ["trace_id"] = Activity.Current?.TraceId.ToString() ?? string.Empty,
            ["http_method"] = context.Request.Method,
            ["http_path"] = context.Request.Path.ToString()
        }))
        {
            _logger.LogInformation("Requisição iniciada");
            await _next(context);
            _logger.LogInformation(
                "Requisição finalizada com status HTTP {StatusCode}",
                context.Response.StatusCode);
        }
    }
}