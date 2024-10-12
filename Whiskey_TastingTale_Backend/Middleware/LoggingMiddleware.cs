namespace Whiskey_TastingTale_Backend.Middleware
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<LoggingMiddleware> _logger;

        public LoggingMiddleware(RequestDelegate next, ILogger<LoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var correlationId = context.Response.Headers["X-Correlation-ID"].ToString();
            var request = context.Request;
            var method = request.Method;
            var path = request.Path;

            _logger.LogInformation($"[API] Request: {method} {path}, CorrelationId: {correlationId}");

            var watch = System.Diagnostics.Stopwatch.StartNew();
            await _next(context); 
            watch.Stop();

            _logger.LogInformation($"[API] Response: {context.Response.StatusCode}, Time taken: {watch.ElapsedMilliseconds} ms, Url: {method} {path}, CorrelationId: {correlationId}");
        }
    }

}
