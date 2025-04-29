namespace CRUDExample.Middleware;

public class ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
{
    public async Task Invoke(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            logger.LogError(
                "Exception caught in {Middleware}\n{ExceptionType}\n{ExceptionMessage}",
                nameof(ExceptionHandlingMiddleware),
                ex.GetType().ToString(),
                ex.Message
            );
            throw;
        }
    }
}
