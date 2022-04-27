using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace WebApiFrameWork.util
{
    public class CorsMiddleware
    {
        private readonly RequestDelegate _next;

        public Task Invoke(HttpContext context)
        {
            return BeginInvoke(context);
        }

        public CorsMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public Task BeginInvoke(HttpContext context)
        {
            context.Response.Headers.Add("Access-Control-Allow-Origin", "*");
           // context.Response.Headers.Add("Access-Control-Allow-Credentials", new[] { "true" });
            context.Response.Headers.Add("Access-Control-Allow-Headers", new[] { "Origin, X-Requested-With, Content-Type, Accept, Athorization, ActualUserOrImpersonatedUserSamAccount, IsImpersonatedUser" });
            context.Response.Headers.Add("Access-Control-Allow-Methods", new[] { "GET, POST, PUT, DELETE, OPTIONS" });
            if (context.Request.Method == HttpMethod.Options.Method)
            {
                context.Response.StatusCode = (int)System.Net.HttpStatusCode.OK;
                return context.Response.WriteAsync("OK");
            }
            return _next.Invoke(context);
        }
    }

    public static class CorsMiddlewareExtensions
    {
        public static IApplicationBuilder UseCorsMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<CorsMiddleware>();
        }
    }
}
