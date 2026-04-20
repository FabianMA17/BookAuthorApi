using Microsoft.AspNetCore.Mvc;

namespace BookAuthorApi.Filters;

public static class ExceptionHelper
{
    public static async Task<IActionResult> HandleExceptionAsync(Func<Task<IActionResult>> action)
    {
        try
        {
            return await action();
        }
        catch (Exception ex)
        {
            var innerMessage = ex.InnerException?.Message ?? ex.Message;
            return new ObjectResult(new
            {
                type = "https://tools.ietf.org/html/rfc9110#section-15.6.1",
                title = "Error interno del servidor",
                status = 500,
                detail = ex.Message,
                innerDetail = innerMessage
            })
            {
                StatusCode = 500
            };
        }
    }

    public static IActionResult HandleException(Func<IActionResult> action)
    {
        try
        {
            return action();
        }
        catch (Exception ex)
        {
            // Log the exception if needed
            // _logger.LogError(ex, "An error occurred while processing the request.");

            var innerMessage = ex.InnerException?.Message ?? ex.Message;
            return new ObjectResult(new
            {
                type = "https://tools.ietf.org/html/rfc9110#section-15.6.1",
                title = "Error interno del servidor",
                status = 500,
                detail = ex.Message,
                innerDetail = innerMessage
            })
            {
                StatusCode = 500
            };
        }
    }
}