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
            // Validate the user credentials (this is just a placeholder, implement your own logic)
            if (user.UserName == "admin" && user.Password == "admin")
            {
                var token = JwtUtils.GenerateJwtToken(user);
                return Ok(new { Token = token });
            }
            return Unauthorized();
        }
    }
}
