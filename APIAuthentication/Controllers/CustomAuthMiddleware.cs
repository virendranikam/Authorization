using APIAuthentication.Helpers.JwtUtils;

namespace APIAuthentication.Controllers
{
    public class CustomAuthMiddleware
    {
        private readonly RequestDelegate _next;

        public CustomAuthMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            UserModel userModel = new UserModel()
            {
                UserName = "admin",
            };
            string token  = JwtUtils.GenerateJwtToken(userModel);
            if (string.IsNullOrEmpty(token))
            {
                httpContext.Response.StatusCode = 401;
                await httpContext.Response.WriteAsync("Token is missing");
                return;
            }
            httpContext.Response.Cookies.Append("token", token, new CookieOptions { HttpOnly = false });
            await _next(httpContext);
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<CustomAuthMiddleware>();
        }
    }
}
