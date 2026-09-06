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
                LogOrderProcessingFailure(ex, StatusCodes.Status400BadRequest);
            await WriteResponseAsync(context, StatusCodes.Status400BadRequest, false, ex.Message, null);
        }
         catch (ArgumentException ex)
        {
                LogOrderProcessingFailure(ex, StatusCodes.Status400BadRequest);
            await WriteResponseAsync(context, StatusCodes.Status400BadRequest, false, ex.Message, null);
        }
         catch (InvalidOperationException ex)
        {
                LogOrderProcessingFailure(ex, StatusCodes.Status400BadRequest);
            await WriteResponseAsync(context, StatusCodes.Status400BadRequest, false, ex.Message, null);
        }
        catch (Exception ex)
        {
                _logger.LogError(
                    new EventId(1002, "OrderProcessingFailure"),
                    ex,
                    "Falha inesperada no processamento da ordem de serviço. Status HTTP: {StatusCode}",
                    StatusCodes.Status500InternalServerError);
            await WriteResponseAsync(context, StatusCodes.Status500InternalServerError, false, "OOPs algo errado aconteceu, tente novamente mais tarde ou entre em contato", null);
        }
    }

        private void LogOrderProcessingFailure(Exception exception, int statusCode)
        {
            _logger.LogWarning(
                new EventId(1001, "OrderProcessingFailure"),
                exception,
                "Falha de domínio no processamento da ordem de serviço. Status HTTP: {StatusCode}",
                statusCode);
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
