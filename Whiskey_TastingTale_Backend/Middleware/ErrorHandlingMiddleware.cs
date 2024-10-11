using Newtonsoft.Json;
using System.Net;

namespace Whiskey_TastingTale_Backend.Middleware
{
    using Microsoft.AspNetCore.Http;
    using Serilog;
    using System;
    using System.Net;
    using System.Threading.Tasks;

    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            var correlationId = context.Response.Headers["X-Correlation-ID"].ToString();

            Log.Error(ex, $"[ERR] CorrelationId: {correlationId}, An unhandled exception occurred while processing the request.");

            // 응답 설정
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            // 클라이언트에게 보낼 오류 응답
            var result = new
            {
                message = "An error occurred while processing your request.",
                details = ex.Message  // 실제 예외 메시지 (개발 환경에서만 보여줌)
            };

            return context.Response.WriteAsJsonAsync(result);
        }
    }


}
