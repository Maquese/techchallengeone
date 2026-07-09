using Application.Models.Responses;
using Domain.Exceptions;

namespace AutoReparaAPI.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (DomainException ex)
        {
            _logger.LogWarning(ex, "Erro de domínio capturado pelo middleware.");
            await WriteResponseAsync(context, StatusCodes.Status400BadRequest, false, ex.Message, null);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro inesperado capturado pelo middleware.");
            await WriteResponseAsync(context, StatusCodes.Status500InternalServerError, false, ex.Message, null);
        }
    }

    private static async Task WriteResponseAsync(HttpContext context, int statusCode, bool success, string message, object? data)
    {
        context.Response.StatusCode = statusCode;
        context.Response.ContentType = "application/json";

        var response = new BaseResponse
        {
            Success = success,
            Message = message,
            Data = data
        };

        await context.Response.WriteAsJsonAsync(response);
    }
}
