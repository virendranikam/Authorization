using APIAuthentication.Helpers.JwtUtils;
using Microsoft.AspNetCore.Mvc;

namespace APIAuthentication.Controllers
{
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public AuthController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        [HttpPost("Login")]
        public IActionResult Login([FromBody] UserModel user)
        {
            if (user.UserName == "virendranikam" && user.Password == "virendranikam")
            {
                user.secreteKey = _configuration.GetSection("Secrete").Value != null ? _configuration.GetSection("Secrete").Value : string.Empty;
                var token = JwtUtils.GenerateJwtToken(user);
                return Ok(new { Token = token });
            }
            return Unauthorized();
        }


    }
}
