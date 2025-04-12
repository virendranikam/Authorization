namespace APIAuthentication.Controllers
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class APIValidationMiddleWare
    {
        private readonly RequestDelegate _next;

        public APIValidationMiddleWare(RequestDelegate next)
        {
            _next = next;
        }

        public Task Invoke(HttpContext httpContext)
        {
            return _next(httpContext);
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class APIValidationMiddleWareExtensions
    {
        public static IApplicationBuilder UseAPIValidationMiddleWare(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<APIValidationMiddleWare>();
        }
    }
}
