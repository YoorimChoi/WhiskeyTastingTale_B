﻿using Serilog.Context;

namespace Whiskey_TastingTale_Backend.Middleware
{
    public class CorrelationIdMiddleware
    {
        private readonly RequestDelegate _next;

        public CorrelationIdMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var correlationId = context.Request.Headers.ContainsKey("X-Correlation-ID")
                ? context.Request.Headers["X-Correlation-ID"].ToString()
                : Guid.NewGuid().ToString();

            context.Response.Headers["X-Correlation-ID"] = correlationId;

            using (LogContext.PushProperty("CorrelationId", correlationId))
            {
                await _next(context);
            }
        }
    }
}

