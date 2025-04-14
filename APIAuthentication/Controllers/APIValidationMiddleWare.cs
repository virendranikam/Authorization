using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace APIAuthentication.Controllers
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class APIValidationMiddleWare
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _configuration;
        public APIValidationMiddleWare(RequestDelegate next, IConfiguration configuration)
        {
            _next = next;
            _configuration = configuration;
        }

        public Task Invoke(HttpContext httpContext)
        {
            List<string> enableRoutes = new List<string>()
                            {
                                "/api/SecureData/GetSecureData",
                                "/api/SecureData/Admin",
                                "/api/Auth/Login"
                            };
            if (enableRoutes.Contains(httpContext.Request.Path.ToString()))
            {
                if (httpContext.Request.Path.ToString().ToLower().Contains("login"))
                {
                    return _next(httpContext);
                }
                else
                {
                    var token = httpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
                    if (string.IsNullOrEmpty(token))
                    {
                        httpContext.Response.StatusCode = 401;
                        return httpContext.Response.WriteAsync("Token is missing");
                    }
                    else
                    {
                        string? secrete = _configuration.GetSection("Secrete").Value;
                        var tokenHandler = new JwtSecurityTokenHandler();
                        var tokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuer = false,
                            ValidateAudience = false,
                            ValidateLifetime = false,
                            ValidateIssuerSigningKey = false,
                            RoleClaimType = ClaimTypes.Role, // To validate based on User Role
                            NameClaimType = ClaimTypes.Name, // To Validate based on Name
                            IssuerSigningKey = new SymmetricSecurityKey(
                                     Encoding.UTF8.GetBytes(secrete))
                        };
                        try
                        {
                            tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken validatedToken);
                            if (validatedToken == null)
                            {
                                httpContext.Response.StatusCode = 401;
                                return httpContext.Response.WriteAsync("Token is not valid");
                            }
                            else
                            {
                                return _next(httpContext);
                            }
                        }
                        catch (SecurityTokenExpiredException)
                        {
                            httpContext.Response.StatusCode = 401;
                            return httpContext.Response.WriteAsync("Token is expired");
                        }
                        catch (SecurityTokenInvalidSignatureException)
                        {
                            httpContext.Response.StatusCode = 401;
                            return httpContext.Response.WriteAsync("Invalid token signature");
                        }
                        catch (Exception ex)
                        {
                            httpContext.Response.StatusCode = 401;
                            return httpContext.Response.WriteAsync("Invalid token");
                        }
                    }
                }
            }
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
