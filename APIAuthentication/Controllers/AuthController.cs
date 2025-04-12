using APIAuthentication.Helpers.JwtUtils;
using Microsoft.AspNetCore.Mvc;

namespace APIAuthentication.Controllers
{
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        [HttpPost("Login")]
        public IActionResult Login([FromBody] UserModel user)
        {
            if (user.UserName == "virendranikam" && user.Password == "virendranikam")
            {
                var token = JwtUtils.GenerateJwtToken(user);
                return Ok(new { Token = token });
            }
            return Unauthorized();
        }
    }
}
